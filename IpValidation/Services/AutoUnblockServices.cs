
using IpValidation.Astracctions;
using IpValidation.Repositories;

namespace IpValidation.Services
{
    public class AutoUnblockServices
    {
        private readonly IBlockingRepository blockingRepository;
        private readonly object Locker = new object();  // no injection it's not service just a normal object

        public AutoUnblockServices(IBlockingRepository blockingRepository)
        {
            this.blockingRepository = blockingRepository;
        }

        public bool TryBlocking(string code,int duration) {

            lock (Locker) {
                if (!blockingRepository.CheckIfBlocked(code))
                {
                    blockingRepository.AddTemporarily(code, duration);
                    return true;
                }
                return false;
            }
            
        }

       
        public void RemoveExpiredBlocks()
        {
            lock (Locker)
            {
                var now = DateTime.Now;
                var expiredKeys = blockingRepository.GetTemporaryBlocks().Where(w => w.Value <= now).Select(w => w.Key).ToList();
                
                foreach (var key in expiredKeys)
                {
                    blockingRepository.GetTemporaryBlocks().TryRemove(key,out _);
                }
            }
        }

    }
}
