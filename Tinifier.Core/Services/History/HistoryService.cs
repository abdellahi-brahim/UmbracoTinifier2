﻿using System;
using System.Collections.Generic;
using System.Linq;
using Tinifier.Core.Models.API;
using Tinifier.Core.Models.Db;
using Tinifier.Core.Models.Services;
using Tinifier.Core.Repository.History;

namespace Tinifier.Core.Services.History
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _historyRepository;

        public HistoryService(IHistoryRepository historyRepository)
        {
            _historyRepository = historyRepository;  
        }

        public void CreateResponseHistory(string timageId, TinyResponse responseItem)
        {
            var newItem = new TinyPNGResponseHistory
            {
                OccuredAt = DateTime.UtcNow,
                IsOptimized = responseItem.Output.IsOptimized,
                ImageId = timageId,
                Error = responseItem.Output.Error,
                Ratio = responseItem.Output.Ratio,
                OptimizedSize = responseItem.Output.Size
            };

            if (responseItem.Input != null)
            {
                newItem.OriginSize = responseItem.Input.Size;
                newItem.Error = string.Empty;
            }

            _historyRepository.Create(newItem);
        }

        public IEnumerable<HistoriesStatisticModel> GetStatisticByDays()
        {
            var histories = _historyRepository.GetAll();

            var historiesByDays = histories.GroupBy(x => x.OccuredAt.Date).
                Select(grp => new HistoriesStatisticModel() {
                    OccuredAt = grp.Key.ToString("MM/dd"),
                    NumberOfOptimized = grp.Count(p => p.Id > 0)
                });

            return historiesByDays;
        }

        public TinyPNGResponseHistory GetImageHistory(string timageId)
        {
            var history = _historyRepository.Get(timageId);

            if(history != null)
            {
                history.OccuredAt = new DateTime(history.OccuredAt.Year, history.OccuredAt.Month,
                                history.OccuredAt.Day, history.OccuredAt.Hour, history.OccuredAt.Minute, history.OccuredAt.Second);
            }
            
            return history;
        }

        public void Delete(string imageId)
        {
            _historyRepository.Delete(imageId);
        }

        public List<TImage> GetImagesWithoutHistory(IEnumerable<TImage> images)
        {
            var imagesList = new List<TImage>();

            foreach (var image in images)
            {
                var imageHistory = _historyRepository.Get(image.Id);

                if (imageHistory == null || ( imageHistory != null && !imageHistory.IsOptimized))
                {
                    imagesList.Add(image);
                }
            }

            return imagesList;
        }

        public IEnumerable<TinyPNGResponseHistory> GetHistoryByPath(string path)
        {
            return _historyRepository.GetHistoryByPath(path);
        }
    }
}
