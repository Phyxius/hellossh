using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace WebAuthnDotNet.Internal
{
    /*
    typedef struct _WEBAUTHN_COSE_CREDENTIAL_PARAMETERS {
        DWORD cCredentialParameters;
        _Field_size_(cCredentialParameters)
        PWEBAUTHN_COSE_CREDENTIAL_PARAMETER pCredentialParameters;
    } WEBAUTHN_COSE_CREDENTIAL_PARAMETERS, *PWEBAUTHN_COSE_CREDENTIAL_PARAMETERS;
    typedef const WEBAUTHN_COSE_CREDENTIAL_PARAMETERS *PCWEBAUTHN_COSE_CREDENTIAL_PARAMETERS;
    */
    [StructLayout(LayoutKind.Sequential)]
    internal struct RawCoseCredentialParameters
    {
        public uint cCredentialParamters { get; set; }
        public RawCoseCredentialParameter[] pCredentialParameters { get; set; }

        public RawCoseCredentialParameters(IEnumerable<RawCoseCredentialParameter> rawCoseCredentialParameters)
        {
            pCredentialParameters = (RawCoseCredentialParameter[])rawCoseCredentialParameters.ToArray().Clone();
            cCredentialParamters = (uint)pCredentialParameters.Length;
        }

        public RawCoseCredentialParameters(IEnumerable<CoseCredentialParameter> coseCredentialParameters)
            : this(coseCredentialParameters.Select((c) => new RawCoseCredentialParameter(c)))
        {

        }
    }
}
