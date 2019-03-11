// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Xunit;

using static System.Tests.Utf8TestUtilities;

namespace System.Tests
{
    public unsafe partial class Utf8StringTests
    {
        [Fact]
        public static void Empty_HasLengthZero()
        {
            Assert.Equal(0, Utf8String.Empty.Length);
            SpanAssert.Equal(ReadOnlySpan<byte>.Empty, Utf8String.Empty.AsBytes());
        }

        [Fact]
        public static void Empty_ReturnsSingleton()
        {
            Assert.Same(Utf8String.Empty, Utf8String.Empty);
        }

        [Fact]
        public static void GetPinnableReference_CalledMultipleTimes_ReturnsSameValue()
        {
            var utf8 = u8("Hello!");

            fixed (byte* pA = utf8)
            fixed (byte* pB = utf8)
            {
                Assert.True(pA == pB);
            }
        }

        [Fact]
        public static void GetPinnableReference_Empty()
        {
            fixed (byte* pStr = Utf8String.Empty)
            {
                Assert.True(pStr != null);
                Assert.Equal((byte)0, *pStr); // should point to null terminator
            }
        }

        [Fact]
        public static void GetPinnableReference_NotEmpty()
        {
            fixed (byte* pStr = u8("Hello!"))
            {
                Assert.True(pStr != null);

                Assert.Equal((byte)'H', pStr[0]);
                Assert.Equal((byte)'e', pStr[1]);
                Assert.Equal((byte)'l', pStr[2]);
                Assert.Equal((byte)'l', pStr[3]);
                Assert.Equal((byte)'o', pStr[4]);
                Assert.Equal((byte)'!', pStr[5]);
                Assert.Equal((byte)'\0', pStr[6]);
            }
        }

        [Theory]
        [InlineData("", true)]
        [InlineData("not empty", false)]
        public static void IsNullOrEmpty(string value, bool expectedIsNullOrEmpty)
        {
            Assert.Equal(expectedIsNullOrEmpty, Utf8String.IsNullOrEmpty(new Utf8String(value)));
        }

        [Fact]
        public static void IsNullOrEmpty_Null_ReturnsTrue()
        {
            Assert.True(Utf8String.IsNullOrEmpty(null));
        }
    }
}
