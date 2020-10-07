using System;
using System.Collections.Generic;
using ChainLib.Crypto;
using ChainLib.Extensions;
using ChainLib.Models;
using ChainLib.Models.Extended;
using ChainLib.Tests.Fixtures;
using Xunit;

namespace ChainLib.Tests
{
	public class WithBlock : 
		IClassFixture<ObjectHashProviderFixture>, 
		IClassFixture<BlockObjectTypeProviderFixture>
	{
		private readonly ObjectHashProviderFixture _hash;
		private readonly BlockObjectTypeProviderFixture _types;

		public WithBlock(ObjectHashProviderFixture hash, BlockObjectTypeProviderFixture types)
		{
			_hash = hash;
			_types = types;
		}

		[Fact]
		public void Empty_object_collection_is_equivalent_to_null()
		{
			var block = new Block
			{
				Nonce = 1,
				PreviousHash = "rosebud".Sha256(),
				Timestamp = (uint) DateTimeOffset.UtcNow.ToUnixTimeSeconds()
			};
			block.MerkleRootHash = block.ComputeMerkleRoot(_hash.Value);
			block.Hash = block.ToHashBytes(_hash.Value);
			
			block.Objects = new List<BlockObject>();
			Assert.Equal(block.Hash, block.ToHashBytes(_hash.Value));
		}

		[Fact]
		public void Consecutive_hashing_is_idempotent()
		{
			var block = new Block
			{
				Nonce = 1,
				PreviousHash = "rosebud".Sha256(),
				Timestamp = (uint) DateTimeOffset.UtcNow.ToUnixTimeSeconds()
			};
			block.MerkleRootHash = block.ComputeMerkleRoot(_hash.Value);
			block.Hash = block.ToHashBytes(_hash.Value);

			Assert.Equal(block.Hash, block.ToHashBytes(_hash.Value));
		}

		[Fact]
		public void Can_round_trip_with_no_objects()
		{
			var block = new Block
			{
				Nonce = 1,
				PreviousHash = "rosebud".Sha256(),
				Timestamp = (uint)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
			};
			block.MerkleRootHash = block.ComputeMerkleRoot(_hash.Value);
			block.Hash = block.ToHashBytes(_hash.Value);

			block.RoundTripCheck(_hash.Value, _types.Value);
		}

		[Fact]
		public void Can_round_trip_with_transaction_objects()
		{
			_types.Value.TryAdd(0, typeof(Transaction));

			var transaction = new Transaction
			{
				Id = $"{Guid.NewGuid()}"
			};
			var blockObject = new BlockObject
			{
				Data = transaction
			};
			blockObject.Hash = blockObject.ToHashBytes(_hash.Value);

			var block = new Block
			{
				Nonce = 1,
				PreviousHash = "rosebud".Sha256(),
				Timestamp = (uint) DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
				Objects = new List<BlockObject> {blockObject}
			};
			block.MerkleRootHash = block.ComputeMerkleRoot(_hash.Value);
			block.Hash = block.ToHashBytes(_hash.Value);

			block.RoundTripCheck(_hash.Value, _types.Value);
		}
	}
}