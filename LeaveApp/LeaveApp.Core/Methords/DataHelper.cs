using AutoMapper;
using System;
using System.Collections.Generic;

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
        public List<DT> AutoMap<ST, DT>(List<ST> data)
        {
            _config = new MapperConfiguration(cfg => {
                cfg.CreateMap<List<ST>, List<DT>>();
            });
            _mapper = _config.CreateMapper();
            return _mapper.Map<List<ST>, List<DT>>(data);
        }

        //public object AutoMap<T1, T2>(T2 issueDetail)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
