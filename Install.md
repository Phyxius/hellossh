# Installing HelloSSH

## Prerequisites
The Windows community package manager [Chocolatey](https://chocolatey.org/install) is recommended but not required. It will make installing the prerequisites more convenient.

HelloSSH targets .NET 5. Install the .NET 5 runtime from [Microsoft](https://dotnet.microsoft.com/download/dotnet/5.0) or via `choco install dotnet-5.0-runtime 5.0.1`. You will also need to [configure Windows Hello](https://support.microsoft.com/en-us/windows/learn-about-windows-hello-and-set-it-up-dae28983-8242-bb2a-d3d1-87c9d265a5f0). Biometric security is not required to use Windows Hello or HelloSSH; you will have to set a PIN as a backup regardless if biometrics are available, and the PIN can always be used instead of a biometric if one is available.

### Common steps

Create a shortcut to `HelloSSH.exe` in `%APPDATA%\Roaming\Microsoft\Windows\Start Menu\Programs\Startup` to have HelloSSH start at boot. Double click the shortcut to run it.

### Using `ssh.exe`
To use the SSH binary that ships with Windows with HelloSSH:

1. The version of SSH that ships with Windows has bugs that prevent it from being compatible with HelloSSH. Newer builds from Microsoft are available but for some reason haven't been included in Windows. You can install them from the [GitHub repo](https://github.com/PowerShell/Win32-OpenSSH) or via `choco install openssh`. You will also need to promote the install directory in your `%PATH%` so that it comes before `System32` by [editing the environment variable](https://www.architectryan.com/2018/08/31/how-to-change-environment-variables-on-windows-10/). Make sure that when you type `ssh -V` in a PowerShell window that it shows the newer version.
2. Set the environment variable `SSH_AUTH_SOCK` to `\\.\pipe\hellossh`. Make sure to change the variable just for your user. You may need to log out and back in for this to fully take effect.
3. Add the line `PubKeyAcceptedKeyTypes -rsa-sha2-512` to `%USERPROFILE%\.ssh\config` (create the file and directory if they don't exist). This will tell SSH to not attempt SHA512-based signatures, which aren't supported by Windows Hello.

### Using SSH Under the Windows Subsystem For Linux (WSL)

To use SSH under WSL, you'll need an external binary to bridge the Windows named pipe implementation to an `AF_UNIX` socket-based one understandable by WSL. Someone else has luckily done this for us already: [wsl-ssh-agent](https://github.com/rupor-github/wsl-ssh-agent). 

Microsoft broke host/guest socket sharing in WSL2, so these instructions differ between WSL1 and WSL2.

#### WSL1

1. Download the latest release from the repository and extract it somewhere you won't delete it. 
2. Make a shortcut to `wsl-ssh-agent-gui.exe` in `%APPDATA%\Roaming\Microsoft\Windows\Start Menu\Programs\Startup`. Change the "Target" field to include the command line arguments `-pipe \\.\pipe\hellossh -socket %USERPROFILE%\ssh-agent.sock`, then run it.
3. In your shell RC file (`.bashrc`, etc.), add the line `export SSH_AUTH_SOCK=/mnt/c/path/to/userprofile/ssh-agent.sock` (change the line to point to the socket created under your Windows home directory).

#### WSL2

1. Download the latest release from the repository and extract `npiperelay.exe`.
2. Copy `npiperelay.exe` into your WSL installation's `~/.ssh` directory (yes, put a Windows binary in your Linux distro's filesystem). Make sure it's executable.
3. Install `socat` in your Linux installation (`sudo apt install socat` on Debian/Ubuntu and friends).
4. Add the following lines to your shell RC file (`.bashrc`, etc.):
    ```bash
    export SSH_AUTH_SOCK=$HOME/.ssh/agent.sock
    ss -a | grep -q $SSH_AUTH_SOCK
    if [ $? -ne 0   ]; then
            rm -f $SSH_AUTH_SOCK
            ( setsid socat UNIX-LISTEN:$SSH_AUTH_SOCK,fork EXEC:"$HOME/.ssh/npiperelay.exe -ei -s //./pipe/hellossh",nofork & ) >/dev/null 2>&1
    fi
    ```
5. Source your shell RC (e.g., `source ~/.bashrc`).

#### Shell Configuration

Once you've connected WSL to the agent, you also need to add the line `PubKeyAcceptedKeyTypes -rsa-sha2-512` to `~/.ssh/config` (create the directory and file if they don't exist). This will tell SSH to not attempt SHA512-based signatures, which aren't supported by Windows Hello.