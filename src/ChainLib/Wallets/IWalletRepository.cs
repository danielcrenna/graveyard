﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChainLib.Wallets
{
    public interface IWalletRepository
    {
        Task<IEnumerable<Wallet>> GetAllAsync();
	    Task<Wallet> GetByIdAsync(string id);
        Task<Wallet> AddAsync(Wallet wallet);
        Task SaveAddressesAsync(Wallet wallet);
    }
}