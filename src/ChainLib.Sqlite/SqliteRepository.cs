﻿using System.IO;
using Microsoft.Extensions.Logging;

namespace ChainLib.Sqlite
{
	public abstract class SqliteRepository
    {
	    private readonly string _baseDirectory;
	    private readonly ILogger _logger;

        private const string DataSubFolder = "Data";

        protected SqliteRepository(string baseDirectory, string subDirectory, string databaseName, ILogger logger)
        {
	        _baseDirectory = baseDirectory;
	        _logger = logger;
            CreateIfNotExists(subDirectory, databaseName);
        }

        protected void CreateIfNotExists(string @namespace, string name)
        {
            var dataDirectory = Path.Combine(_baseDirectory, DataSubFolder);

            if (!Directory.Exists(dataDirectory))
                Directory.CreateDirectory(dataDirectory);

            if (!Directory.Exists(Path.Combine(dataDirectory, @namespace)))
                Directory.CreateDirectory(Path.Combine(dataDirectory, @namespace));
            
            DataFile = Path.Combine(dataDirectory, @namespace, $"{name}.db3");

            if (File.Exists(DataFile))
                return;

            _logger?.LogInformation($"Creating and migrating database at '{DataFile}'");
            MigrateToLatest();
        }

        protected string DataFile { get; private set; }

	    public abstract void MigrateToLatest();

        public void Purge()
        {
            _logger?.LogInformation($"Deleting database at '{DataFile}'");
            File.Delete(DataFile);

            var directoryName = Path.GetDirectoryName(DataFile);
            if (Directory.GetFiles(directoryName, "*.*", SearchOption.AllDirectories).Length > 0)
            {
                _logger?.LogInformation($"Deleting database directory '{directoryName}' as it is no longer in use");
                Directory.Delete(directoryName, true);
            }
        }
    }
}