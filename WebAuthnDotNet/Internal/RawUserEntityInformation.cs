using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAuthnDotNet.Internal
{
    /*
    //+------------------------------------------------------------------------------------------
    // Information about an User Entity
    //-------------------------------------------------------------------------------------------
    #define WEBAUTHN_MAX_USER_ID_LENGTH                             64

    #define WEBAUTHN_USER_ENTITY_INFORMATION_CURRENT_VERSION        1

    typedef struct _WEBAUTHN_USER_ENTITY_INFORMATION {
        // Version of this structure, to allow for modifications in the future.
        // This field is required and should be set to CURRENT_VERSION above.
        DWORD dwVersion;

        // Identifier for the User. This field is required.
        DWORD cbId;
        _Field_size_bytes_(cbId)
        PBYTE pbId;

        // Contains a detailed name for this account, such as "john.p.smith@example.com".
        PCWSTR pwszName;

        // Optional URL that can be used to retrieve an image containing the user's current avatar,
        // or a data URI that contains the image data.
        PCWSTR pwszIcon;

        // For User: Contains the friendly name associated with the user account by the Relying Party, such as "John P. Smith".
        PCWSTR pwszDisplayName;
    } WEBAUTHN_USER_ENTITY_INFORMATION, *PWEBAUTHN_USER_ENTITY_INFORMATION;
    typedef const WEBAUTHN_USER_ENTITY_INFORMATION *PCWEBAUTHN_USER_ENTITY_INFORMATION;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawUserEntityInformation
    {
        public uint dwVersion { get; set; }
        public uint cbId { get; set; }
        public byte[] pbId { get; set; }
        public string pwszName { get; set; }
        public string pwszIcon { get; set; }
        public string pwszDisplayName { get; set; }

        public RawUserEntityInformation(UserEntityInformation template)
        {
            dwVersion = (uint)template.Version;
            cbId = (uint)template.Id.Length;
            pbId = (byte[])template.Id.Clone();
            pwszName = template.Name;
            pwszIcon = template.Icon;
            pwszDisplayName = template.DisplayName;
        }
    }
}
