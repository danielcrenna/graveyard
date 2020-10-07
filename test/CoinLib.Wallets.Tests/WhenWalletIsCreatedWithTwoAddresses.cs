using CoinLib.Wallets.Tests.Fixtures;
using Crypto.Shim;
using Xunit;

namespace CoinLib.Wallets.Tests
{
	public class WhenWalletIsCreatedWithTwoAddresses :
		IClassFixture<WalletWithTwoAddressesFixture>,
		IClassFixture<WifAddressStorageFormatFixture>,
		IClassFixture<KeyStoreStorageFormatFixture>
	{
		private readonly WifAddressStorageFormatFixture _wif;
		private readonly KeyStoreStorageFormatFixture _keystore;

		public WhenWalletIsCreatedWithTwoAddresses(
			WalletWithTwoAddressesFixture fixture,
			WifAddressStorageFormatFixture wif, 
			KeyStoreStorageFormatFixture keystore)
		{
			_wif = wif;

			_keystore = keystore;
			Fixture = fixture;
		}
		
		public WalletWithTwoAddressesFixture Fixture { get; set; }

		[Fact]
		public void There_are_two_keypairs_in_the_wallet()
		{
			Assert.NotNull(Fixture.Value);
			Assert.Equal(2, Fixture.Value.KeyPairs.Count);
		}

		[Fact]
		public void Both_addresses_can_be_exported_via_WIF()
		{
			var wallet1 = Fixture.Value;
			var wif1 = _wif.Value.Export(wallet1, wallet1.KeyPairs[0].PublicKey);
			var wif2 = _wif.Value.Export(wallet1, wallet1.KeyPairs[1].PublicKey);

			Assert.Equal(wallet1.KeyPairs[0].PrivateKey.ToHex(), WifAddressStorageFormat.GetPrivateKeyFromImport(wif1).ToHex());
			Assert.Equal(wallet1.KeyPairs[1].PrivateKey.ToHex(), WifAddressStorageFormat.GetPrivateKeyFromImport(wif2).ToHex());
		}

		[Fact]
		public void Both_addresses_can_be_imported_via_WIF()
		{
			var wallet1 = Fixture.Value;
			var wif1 = _wif.Value.Export(wallet1, wallet1.KeyPairs[0].PublicKey);
			var wif2 = _wif.Value.Export(wallet1, wallet1.KeyPairs[1].PublicKey);

			var factory = new FixedSaltWalletFactoryProvider("_NaiveCoin_Salt_");
			var wallet2 = factory.Create("rosebud");
			_wif.Value.Import(wallet2, wif1);
			_wif.Value.Import(wallet2, wif2);

			Assert.Equal(wallet1.KeyPairs.Count, wallet2.KeyPairs.Count);
			Assert.Equal(wallet1.KeyPairs[0].PublicKey.ToHex(), wallet2.KeyPairs[0].PublicKey.ToHex());
			Assert.Equal(wallet1.KeyPairs[0].PrivateKey.ToHex(), wallet2.KeyPairs[0].PrivateKey.ToHex());
			Assert.Equal(wallet1.KeyPairs[1].PublicKey.ToHex(), wallet2.KeyPairs[1].PublicKey.ToHex());
			Assert.Equal(wallet1.KeyPairs[1].PrivateKey.ToHex(), wallet2.KeyPairs[1].PrivateKey.ToHex());
		}

		[Fact]
		public void Both_addresses_can_be_imported_and_exported_via_keystore()
		{
			var wallet1 = Fixture.Value;
			var kstore1 = _keystore.Value.Export(wallet1, wallet1.KeyPairs[0].PublicKey);
			var kstore2 = _keystore.Value.Export(wallet1, wallet1.KeyPairs[1].PublicKey);
			Assert.NotNull(kstore1);
			Assert.NotNull(kstore2);
			
			var factory = new FixedSaltWalletFactoryProvider("_NaiveCoin_Salt_");
			var wallet2 = factory.Create("rosebud");
			_keystore.Value.Import(wallet2, kstore1);
			_keystore.Value.Import(wallet2, kstore2);

			Assert.Equal(wallet1.KeyPairs.Count, wallet2.KeyPairs.Count);
			Assert.Equal(wallet1.KeyPairs[0].PublicKey.ToHex(), wallet2.KeyPairs[0].PublicKey.ToHex());
			Assert.Equal(wallet1.KeyPairs[0].PrivateKey.ToHex(), wallet2.KeyPairs[0].PrivateKey.ToHex());
			Assert.Equal(wallet1.KeyPairs[1].PublicKey.ToHex(), wallet2.KeyPairs[1].PublicKey.ToHex());
			Assert.Equal(wallet1.KeyPairs[1].PrivateKey.ToHex(), wallet2.KeyPairs[1].PrivateKey.ToHex());
		}
	}
}