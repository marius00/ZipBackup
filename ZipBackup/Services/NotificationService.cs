﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ZipBackup.Services {
    public class NotificationService {
        private DateTime _lastCacheClearDate = DateTime.Now;
        private readonly ConcurrentDictionary<string, bool> _cache = new();
        private readonly ConcurrentBag<string> _notifications = new();

        public void Add(string component, string content) {
            if ("Core".Equals(component)) {
                var key = component + content;
                if (!_cache.ContainsKey(key)) {
                    _cache.TryAdd(key, true);
                    _notifications.Add(content);
                }
            }
            else {
                if (!_cache.ContainsKey(component)) {
                    _cache.TryAdd(component, true);
                    _notifications.Add(content);
                }
            }
        }

        public List<string> GetNotifications() {
            return _notifications.ToList();
        }

        public void Clear() {
            _notifications.Clear();

            // Not using UTC as we care about time, and don't want to spam notifications at midnight.
            if (_lastCacheClearDate.Date != DateTime.Now.Date) {
                if (DateTime.Now.Hour > 9) {
                    _lastCacheClearDate = DateTime.Now;
                    _cache.Clear();
                }
            }
        }

    }
}
