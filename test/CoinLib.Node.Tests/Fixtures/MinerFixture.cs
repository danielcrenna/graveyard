using CoinLib.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CoinLib.Models;
using CoinLib.Node.Tests.Fixtures;
using CoinLib.Services;
using Xunit;

namespace CoinLib.Tests.Fixtures
{
	[Collection("Blockchain")]
	public class MinerFixture
	{
		public MinerFixture(BlockchainFixture blockchain)
		{
			var coinSettings = new CoinSettingsFixture();
			var coinSettingsOptions = new OptionsWrapper<CoinSettings>(coinSettings.Value);
			var hashProvider = new HashProviderFixture();
			var pow = new CoinBasedProofOfWork(coinSettingsOptions, hashProvider.Value);
			
			var factory = new LoggerFactory();
			factory.AddConsole();

			Value = new Miner(blockchain.Value, pow, coinSettingsOptions, factory.CreateLogger<Miner>());
		}

		public Miner Value { get; set; }
	}
}