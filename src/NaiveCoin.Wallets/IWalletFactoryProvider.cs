﻿namespace NaiveCoin.Wallets
{
    public interface IWalletFactoryProvider
    {
        Wallet Create(string password);
    }
}