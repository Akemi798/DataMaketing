using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OkVip.ManagementDataMarketing.Commons.Constants;
using OkVip.ManagementDataMarketing.Data;
using OkVip.ManagementDataMarketing.Models.DbModels;
using OkVip.ManagementDataMarketing.Models.ViewModels.Account;
using OkVip.ManagementDataMarketing.Models.ViewModels.DataImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OkVip.ManagementDataMarketing.Services
{
    public interface IDataMarketingServices
    {
        public List<DataMarketingViewModel> GetAll();
        public DataMarketingViewModel GetById(string id);
        public void Create(CreateDataMarketingModel model);

        public List<DataMarketingViewModel> GetAllNoDuplicate();
        public List<DataMarketingViewModel> GetDuplicateData();

        public bool DeleteDuplicate();
        public bool CheckExist(string phoneNumber);
        public void Insert(List<DataMarketing> data);
    }

    public class DataMarketingServices : IDataMarketingServices
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public DataMarketingServices(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public void Create(CreateDataMarketingModel model)
        {
            throw new System.NotImplementedException();
        }

        public List<DataMarketingViewModel> GetAll()
        {
            List<DataMarketing> listDatas = _db.DataMarketing.Take(500).AsNoTracking().ToList();
            List<DataMarketingViewModel> result = new List<DataMarketingViewModel>();

            for (int i = 0; i < listDatas.Count; i++)
            {
                DataMarketingViewModel resultItem = _mapper.Map<DataMarketingViewModel>(listDatas[i]);
                result.Add(resultItem);
            }
            return result;
        }

        public List<DataMarketingViewModel> GetAllNoDuplicate()
        {
            List<DataMarketing> listDatas = _db.DataMarketing.Where(x => x.IsDuplicate == false && x.CreatedDate.Date == DateTime.Now.Date).AsNoTracking().ToList();
            List<DataMarketingViewModel> result = new List<DataMarketingViewModel>();

            for (int i = 0; i < listDatas.Count; i++)
            {
                DataMarketingViewModel resultItem = _mapper.Map<DataMarketingViewModel>(listDatas[i]);
                result.Add(resultItem);
            }
            return result;
        }

        public List<DataMarketingViewModel> GetDuplicateData()
        {
            List<DataMarketing> listDatas = _db.DataMarketing.Where(x => x.IsDuplicate == true && x.CreatedDate.Date == DateTime.Now.Date).AsNoTracking().ToList();
            List<DataMarketingViewModel> result = new List<DataMarketingViewModel>();

            for (int i = 0; i < listDatas.Count; i++)
            {
                DataMarketingViewModel resultItem = _mapper.Map<DataMarketingViewModel>(listDatas[i]);
                result.Add(resultItem);
            }
            return result;
        }

        public bool CheckExist(string phoneNumber)
        {
            if(_db.DataMarketing.Any(x => x.PhoneNumber == phoneNumber))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Insert(List<DataMarketing> data)
        {
            _db.DataMarketing.AddRangeAsync(data);
            _db.SaveChanges();
        }

        public bool DeleteDuplicate()
        {
            List<DataMarketing> listDatas = _db.DataMarketing.Where(x => x.IsDuplicate == true).AsNoTracking().ToList();
            _db.DataMarketing.RemoveRange(listDatas);
            _db.SaveChanges();
            return true;
        }

        public DataMarketingViewModel GetById(string id)
        {
            throw new System.NotImplementedException();
        }

    
    }
}
