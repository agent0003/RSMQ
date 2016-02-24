using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSMQ
{
    public class RSMQ
    {
        public string RedisNameSpace { get; set; }
        public ConnectionMultiplexer RedisConnection { get; set; }
        public int RedisDbIndex { get; set; }

        public RSMQ(ConnectionMultiplexer redisConnection, int redisDbIndex = -1)
        {
            RedisNameSpace = "rsmq";
            RedisConnection = redisConnection;
            RedisDbIndex = redisDbIndex;
        }

        /// <summary>
        /// Send message to queue
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public string SendMessage(MessageOptions options)
        {
            if (String.IsNullOrEmpty(options.QueueName))
                throw new ArgumentException("QueueName");

            var queue = _GetQueue(options.QueueName, true);
            if (queue == null)
                return null;

            var db = RedisConnection.GetDatabase(RedisDbIndex);

            db.SortedSetAdd(RedisNameSpace + ":" + options.QueueName, queue.Uid, queue.Ts + options.Delay * 1000);
            db.HashSet(RedisNameSpace + ":" + options.QueueName + ":Q", queue.Uid, options.Message);
            db.HashIncrement(RedisNameSpace + ":" + options.QueueName + ":Q", "totalsent");
            return queue.Uid;
        }





        Queue _GetQueue(string queueName, bool includeUid)
        {
            IServer server = RedisConnection.GetServer(RedisConnection.GetEndPoints().First());
            var db = RedisConnection.GetDatabase(RedisDbIndex);
            //var time = db.ti
            var values = db.HashGet(RedisNameSpace + ":" + queueName + ":Q", new RedisValue[] { "vt", "delay", "maxsize" });
            if (!values[0].HasValue)
                return null;

            var time = server.Time();

            var vt = (int)values[0];
            var delay = (int)values[1];
            var maxsize = (int)values[2];
            long microseconds = time.Ticks / (TimeSpan.TicksPerMillisecond / 1000);
            var ms = microseconds.ToString().Substring(0, 6).PadLeft(6, '0');
            var ts = long.Parse(_ConvertToUnixTimestamp(DateTime.Now).ToString() + ms.Substring(0, 3));

            var q = new Queue
            {
                Vt = vt,
                Delay = delay,
                Maxsize = maxsize,
                Ts = ts
            };
            
            if (includeUid)
            {
                var uid = _Makeid(22);
                q.Uid = Base36.Encode(long.Parse(_ConvertToUnixTimestamp(time).ToString() + ms)) + uid;
            }

            return q;
        }


        double _ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }


        string _Makeid(int len)
        {
            var i = 0;
            int _i = 0;
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            var rand = new Random();
            for (i = _i = 0; 0 <= len ? _i < len : _i > len; i = 0 <= len ? ++_i : --_i)
            {
                text += possible[(int)(Math.Floor((double)rand.Next(possible.Length)))];
            }
            return text;
        }

        

    }
}
