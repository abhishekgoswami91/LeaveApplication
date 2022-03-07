using AutoMapper;

namespace LeaveApp.Core.Methords
{
    public class DataHelper
    {
        private MapperConfiguration _config;
        private IMapper _mapper;
        public DataHelper()
        {
            
        }
        public DT AutoMap<ST, DT>(ST data)
        {
            _config = new MapperConfiguration(cfg => {
                cfg.CreateMap<ST, DT>();
            });
            _mapper = _config.CreateMapper();
            return _mapper.Map<ST, DT>(data);
        }
        

    }
}
