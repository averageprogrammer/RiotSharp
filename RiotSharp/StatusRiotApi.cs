﻿using Newtonsoft.Json;
using RiotSharp.Http;
using RiotSharp.Http.Interfaces;
using RiotSharp.Interfaces;
using RiotSharp.StatusEndpoint;
using System.Threading.Tasks;
using RiotSharp.Misc;
using System;

namespace RiotSharp
{
    public class StatusRiotApi : IStatusRiotApi
    {
        #region Private Fields
        
        private const string StatusRootUrl = "/lol/status/v3/shard-data";

        private const string PlatformSubdomain = "{0}.";

        private const string RootDomain = "api.riotgames.com";

        private IRequester requester;

        private static StatusRiotApi instance;

        #endregion

        /// <summary>
        /// Get the instance of StatusRiotApi.
        /// </summary>
        /// <param name="apiKey">The api key.</param>
        /// <returns>The instance of StatusRiotApi.</returns>
        public static StatusRiotApi GetInstance(string apiKey)
        {
            if (instance == null)
                instance = new StatusRiotApi(apiKey);
            return instance;
        }

        private StatusRiotApi(string apiKey)
        {
            Requesters.StatusApiRequester = new Requester(apiKey);
            requester = Requesters.StatusApiRequester;
        }

        public StatusRiotApi(IRequester requester)
        {
            if (requester == null)
                throw new ArgumentNullException(nameof(requester));
            this.requester = requester;
        }

        #region Public Methods      

        public ShardStatus GetShardStatus(Region region)
        {
            var json = requester.CreateGetRequest(StatusRootUrl, region, null, true);

            return JsonConvert.DeserializeObject<ShardStatus>(json);
        }

        public async Task<ShardStatus> GetShardStatusAsync(Region region)
        {
            var json = await requester.CreateGetRequestAsync(StatusRootUrl, region, null, true);

            return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ShardStatus>(json));
        }

        #endregion
    }
}
