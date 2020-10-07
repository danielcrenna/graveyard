using System.Collections.Generic;
using ChainLib.Tests.Fixtures;
using Xunit;

namespace ChainLib.Tests
{
	public class WhenObjectIsHashed : IClassFixture<ObjectHashProviderFixture>
    {
        private readonly ObjectHashProviderFixture _provider;

        public WhenObjectIsHashed(ObjectHashProviderFixture provider)
        {
            _provider = provider;
        }

        [Fact]
        public void Property_order_doesnt_matter()
        {
            var foo = new { Foo = "A", Bar = "B" };
            var bar = new { Bar = "B", Foo = "A" };

            Assert.Equal
            (
                _provider.Value.ComputeHashString(foo), 
                _provider.Value.ComputeHashString(bar)
            );
        }
		
        [Fact]
        public void Properties_that_dont_equal_have_different_hashes()
        {
            var foo = new { Foo = "A", Bar = "A" };
            var bar = new { Bar = "B", Foo = "A" };

            Assert.NotEqual
            (
                _provider.Value.ComputeHashString(foo),
                _provider.Value.ComputeHashString(bar)
            );
        }
		
        [Fact]
        public void Properties_with_no_value_dont_matter()
        {
            var foo = new Stub  { A = "A", B = "B" };
            var bar = new Stub  { B = "B", A = "A", C = null };

            Assert.Equal
            (
                _provider.Value.ComputeHashString(foo),
                _provider.Value.ComputeHashString(bar)
            );
        }

	    [Fact]
	    public void Null_collections_are_equivalent_to_empty_collections()
	    {
		    var foo = new Stub { D = null };
		    var bar = new Stub { D = new List<string>() };

		    var expected = _provider.Value.ComputeHashString(foo);
		    var actual = _provider.Value.ComputeHashString(bar);

		    Assert.Equal
		    (
			    expected,
			    actual
		    );
	    }

		private struct Stub
        {
            public string A { get; set; }
            public string B { get; set; }
            public string C { get; set; }
			public List<string> D { get; set; }
        }
    }
}
