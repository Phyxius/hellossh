# HelloSSH &mdash; A Windows Hello SSH Agent
HelloSSH is an SSH Agent that uses [Windows Hello](https://blogs.windows.com/windowsdeveloper/2016/01/26/convenient-two-factor-authentication-with-microsoft-passport-and-windows-hello/) to create and store SSH keys. It's compatibile with any PC running a recent build of Windows 10 and automatically protects keys with the highest level of protection available on the system, no complex passphrases required. 

## Why Windows Hello
In a nutshell, Windows Hello is Microsoft's attempt to eliminate password-based authentication schemes. Instead, each user logs in with a public/private keypair &mdash; exactly like SSH. When an application wants to use the keypair, the user is prompted to allow it by entering their PIN or using any configured biometrics such as a fingerprint. They key itself as well as the PIN and any biometric authentication data is protected using the highest level of protection available on the system. On a PC equipped with a [Trusted Platform Module](https://en.wikipedia.org/wiki/Trusted_Platform_Module) (this is most PCs nowadays; they've been required for new devices shipping Windows for a while, and both Intel and AMD ship firmware TPM implementations), the key is stored in it. That means that even if an attacker were to completely take over your computer, they would be unable to export the key; they'd still be able to *use* the key while they mainatained access to your machine, but they wouldn't be able to do anything offline, increasing the chance of detection. With a traditional agent, an attacker that can read the passphrase out of memory (for instance, by becoming `root`), could just read the passphrase out of the agent's memory and then steal the key file.

If your PC doesn't have a TPM, Windows will fall back on successively weaker software-based methods. These are less secure than the TPM-based protection, but are at worst at parity with a traditional agent, and depending on what other hardware features are available may be better.

## Technical Details and Limitations
HelloSSH offers a very limited subset of the full SSH Agent protocol. In particular, it only offers two operations: listing identities, and signing challenges. All other operations will fail. This means you can't use it with any existing keys; if you want to do that, you can use the `SSH_CONFIG` file to set which host uses which agent socket.

HelloSSH exposes only operations made available by the Windows Hello APIs (AKA [`KeyCredentialManager`](https://docs.microsoft.com/en-us/uwp/api/windows.security.credentials.keycredentialmanager?view=winrt-19041) and friends). This means that generated keys have the following limitations:

1. Keys are always 2048-bit RSA keys. Yes, I would prefer ECDSA or ed25519, but that is what is available. RSA keys are still considered secure as of this writing, so that's not a dealbreaker.
2. The only signature type supported is `rsa-sha2-256`. The default signature type requested by OpenSSH is `rsa-sha2-512`, which will fail. You'll have to configure it to not request that kind, or you won't be able to log in anywhere.
3. Keys are not exportable. Ever. Make sure you have a backup way into any servers. I personally keep a regular SSH key on an offline file store that is added to all my hosts, just in case. 
4. If you change your Windows PIN, reset your TPM, or (depending on your hardware) update your BIOS, your keys might be erased. This probably won't happen during the regular courses of use, but you should always have a backup method to access your servers just in case (see above).
5. You will be prompted to authenticate for *every* use of the key. This can get a little annoying, but Microsoft doesn't provide an option for timed access. 

See [Security](SECURITY.md) for more information about HelloSSH's threat model and technical details.

## Installation and Prerequisites
See [Install](Install.md).
## Using HelloSSH
To use HelloSSH, simply run it. The first time, you'll get a prompt to set up a key with a default name. If you want to use a different name, or create multiple keys, you can access the management UI by double clicking on the HelloSSH system tray icon (it's the blue square with the key and padlock). You'll also have to configure your system's SSH binaries to use it instead of the agent that ships with Windows.

Once you've generated some keys, you can also use the key management UI to export individual fingerprints or all of them at once. You can also list them with `ssh-add -L` (after setting up SSH, see below).

## Alternatives

If you are looking to use existing hardware security devices (such as Smart Cards, [TPM-backed Windows certificates](https://polansky.co/blog/tpm-backed-certificates-windows/), or YubiKeys), consider using [WinCryptSSHAgent](https://github.com/buptczq/WinCryptSSHAgent), which uses the Windows Certificate APIs for key operations. 