﻿using System;
using System.Diagnostics.Contracts;

namespace ChainLib.Wallets.Factories
{
    public class FixedSaltWalletFactoryProvider : IWalletFactoryProvider
    {
        private readonly string _salt;

        public FixedSaltWalletFactoryProvider(string salt)
        {
	        Contract.Assert(!string.IsNullOrWhiteSpace(salt));
			Contract.Assert(salt.Length == 16);
			_salt = salt;
        }

        public Wallet Create(string password)
        {
			Contract.Assert(!string.IsNullOrWhiteSpace(password));
			return Wallet.FromPassword(password, _salt);
        }

	    public Wallet Create(params object[] args)
	    {
		    return args.Length != 1 ? null : Create(args[0]?.ToString());
	    }
    }
}