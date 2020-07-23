﻿using Tinifier.Core.Infrastructure;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using NPoco;

namespace Tinifier.Core.Models.Db
{
    [TableName(PackageConstants.DbTinifierImageHistoryTable)]
    [PrimaryKey("Id", AutoIncrement = true)]
    public class TinifierImagesHistory
    {
        [PrimaryKeyColumn(AutoIncrement = true)]
        public int Id { get; set; }

        public string ImageId { get; set; }

        public string OriginFilePath { get; set; }
    }
}
