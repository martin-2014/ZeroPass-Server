using System;
using ZeroPass.Model;

namespace ZeroPass.Service
{
    internal class Randomizer : IRandom
    {
        readonly Random Generator = new Random();

        public string GenerateVerificationCode()
            => Generator.Next(100000, 999999).ToString();
    }
}
