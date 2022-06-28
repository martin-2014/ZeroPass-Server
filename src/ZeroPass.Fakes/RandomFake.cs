using System;
using ZeroPass.Model;

namespace ZeroPass.Fakes
{
    public class RandomFake : IRandom
    {
        public string GeneratedCode { get; private set; }

        public string GenerateVerificationCode()
        {
            GeneratedCode = DateTime.Now.ToString();
            return GeneratedCode;
        }
    }
}
