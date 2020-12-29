using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAuthnDotNet.Internal
{
    /*
    //+------------------------------------------------------------------------------------------
    // Information about an RP Entity
    //-------------------------------------------------------------------------------------------

    #define WEBAUTHN_RP_ENTITY_INFORMATION_CURRENT_VERSION          1

    typedef struct _WEBAUTHN_RP_ENTITY_INFORMATION {
        // Version of this structure, to allow for modifications in the future.
        // This field is required and should be set to CURRENT_VERSION above.
        DWORD dwVersion;

        // Identifier for the RP. This field is required.
        PCWSTR pwszId;

        // Contains the friendly name of the Relying Party, such as "Acme Corporation", "Widgets Inc" or "Awesome Site".
        // This field is required.
        PCWSTR pwszName;

        // Optional URL pointing to RP's logo. 
        PCWSTR pwszIcon;
    } WEBAUTHN_RP_ENTITY_INFORMATION, *PWEBAUTHN_RP_ENTITY_INFORMATION;
    typedef const WEBAUTHN_RP_ENTITY_INFORMATION *PCWEBAUTHN_RP_ENTITY_INFORMATION;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawRPEntityInformation
    {
        public uint dwVersion { get; set; }
        public string pwszId { get; set; }
        public string pwszName { get; set; }
        public string pwszIcon { get; set; }

        public RawRPEntityInformation(RPEntityInformation template)
        {
            dwVersion = (uint)template.Version;
            pwszId = template.Id;
            pwszName = template.Name;
            pwszIcon = template.Icon;
        }
    }
}
