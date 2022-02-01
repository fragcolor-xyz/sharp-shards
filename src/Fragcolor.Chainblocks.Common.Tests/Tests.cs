/* SPDX-License-Identifier: BSD-3-Clause */
/* Copyright � 2022 Fragcolor Pte. Ltd. */

using System.Threading;

using NUnit.Framework;

namespace Fragcolor.Chainblocks.Tests
{
    [TestFixture]
    public class Tests : TestBase
    {
        [Test]
        public void Test1()
        {
            var chain = new Variable();
            Env.Eval(@"(Chain ""test"" :Looped (Msg ""XXX"") .position (Log) (Pause 1.0))", chain.Ptr);

            var float4 = new Float4 { x = 3, y = 4, z = 5, w = 0};
            var position = new ExternalVariable(chain.Value.chain, "position");
            position.Value.float4 = float4;
            position.Value.type = CBType.Float4;
            position.Value.flags = (1 << 2);

            Native.Core.Schedule(Node, chain.Value.chain);

            for (var i = 0; i < 50; i++)
            {
                Thread.Sleep(100);
                position.Value.float4.w = i;
                Tick();
            }

            Native.Core.Unschedule(Node, chain.Value.chain);
        }
    }
}