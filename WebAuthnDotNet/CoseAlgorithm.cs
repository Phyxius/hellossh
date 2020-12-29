using System;
using System.Collections.Generic;
using System.Text;

namespace WebAuthnDotNet
{
    public enum CoseAlgorithm : int
    {
        EcdsaP256WithSha256 = -7,
        EcdsaP384WithSha384 = -35,
        EcdsaP521WithSha512 = -36,

        RsaSsaPkcs1V1Dot5WithSha256 = -257,
        RsaSsaPkcs1V1Dot5WithSha384 = -258,
        RsaSsaPkcs1V1Dot5WithSha512 = -259,

        RsaPssWithSha256 = -37,
        RsaPssWithSha384 = -38,
        RsaPssWithSha512 = -39,
    }
}
