using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;

namespace PharmaGo.BusinessLogic
{
    public class StockRequestManager : IStockRequestManager
    {
        private readonly IRepository<StockRequest> _stockRequestRepository;
        private readonly IRepository<User> _employeeRepository;
        private readonly IRepository<Drug> _drugRepository;
        private readonly IRepository<Session> _sessionRepository;

        public StockRequestManager(IRepository<StockRequest> stockRequestRepository,
            IRepository<User> employeeRepository, IRepository<Drug> drugRepository, IRepository<Session> sessionRepository)
        {
            this._stockRequestRepository = stockRequestRepository;
            this._employeeRepository = employeeRepository;
            this._drugRepository = drugRepository;
            this._sessionRepository = sessionRepository;
        }

        public bool ApproveStockRequest(int id)
        {
            var stockRequest = this._stockRequestRepository.GetOneByExpression(s => s.Id == id);
            if (stockRequest == null)
            {
                throw new InvalidResourceException("Invalid stock request.");
            }

            if (stockRequest != null)
            {
                if (stockRequest.Status == Domain.Enums.StockRequestStatus.Approved)
                {
                    throw new InvalidResourceException("Stock request already approved.");
                }

                if (stockRequest.Status == Domain.Enums.StockRequestStatus.Rejected)
                {
                    throw new InvalidResourceException("Stock request already rejected.");
                }
            }

            foreach (var stockRequestDetail in stockRequest.Details)
            {
                if (stockRequestDetail.Drug.Deleted == true)
                {
                    throw new InvalidResourceException("Stock request has deleted drugs included.");
                }

                var drug = this._drugRepository.GetOneByExpression(d => d.Id == stockRequestDetail.Drug.Id);
                drug.Stock += stockRequestDetail.Quantity;
                this._drugRepository.UpdateOne(drug);
            }

            stockRequest.Status = Domain.Enums.StockRequestStatus.Approved;
            this._stockRequestRepository.UpdateOne(stockRequest);

            this._drugRepository.Save();
            this._stockRequestRepository.Save();

            return true;
        }

        public bool RejectStockRequest(int id)
        {
            var stockRequest = this._stockRequestRepository.GetOneByExpression(s => s.Id == id);
            if (stockRequest == null)
            {
                throw new InvalidResourceException("Invalid stock request.");
            }

            if (stockRequest != null)
            {
                if (stockRequest.Status == Domain.Enums.StockRequestStatus.Approved)
                {
                    throw new InvalidResourceException("Stock request already approved.");
                }

                if (stockRequest.Status == Domain.Enums.StockRequestStatus.Rejected)
                {
                    throw new InvalidResourceException("Stock request already rejected.");
                }
            }

            stockRequest.Status = Domain.Enums.StockRequestStatus.Rejected;
            this._stockRequestRepository.UpdateOne(stockRequest);
            this._stockRequestRepository.Save();

            return true;
        }

        public StockRequest CreateStockRequest(StockRequest stockRequest, string token)
        {
            User? existEmployee = null;
            if (stockRequest.Details == null)
            {
                throw new InvalidResourceException("Invalid stock details.");
            }

            if (stockRequest.Details.Count == 0)
            {
                throw new InvalidResourceException("Invalid stock details.");
            }

            var session = this._sessionRepository.GetOneByExpression(session => session.Token == new Guid(token));
            if (session == null)
            {
                throw new InvalidResourceException("Invalid session from employee.");
            }

            var userId = session.UserId;

            existEmployee = this._employeeRepository.GetOneDetailByExpression(u => u.Id == userId);
            if (existEmployee == null)
            {
                throw new InvalidResourceException("Invalid employee.");
            }

            stockRequest.Employee = existEmployee;

            foreach (StockRequestDetail item in stockRequest.Details)
            {
                var drug = this._drugRepository.GetOneByExpression(d => d.Code == item.Drug.Code && d.Pharmacy.Id == existEmployee.Pharmacy.Id);
                if (drug == null)
                {
                    throw new InvalidResourceException("Stock request has invalid drug.");
                }
                if (item.Quantity < 1)
                {
                    throw new InvalidResourceException("Stock request has negative quantity.");
                }

                item.Drug = drug;
            }

            stockRequest.Status = Domain.Enums.StockRequestStatus.Pending;
            this._stockRequestRepository.InsertOne(stockRequest);
            this._stockRequestRepository.Save();

            return stockRequest;
        }

        public IEnumerable<StockRequest> GetStockRequestsByEmployee(string token, StockRequestSearchCriteria searchCriteria)
        {
            if (string.IsNullOrEmpty(token.ToString()))
            {
                throw new InvalidResourceException("Invalid employee.");
            }

            var session = this._sessionRepository.GetOneByExpression(session => session.Token == new Guid(token));
            if (session == null)
            {
                throw new InvalidResourceException("Invalid employee.");
            }

            searchCriteria.EmployeeId = session.UserId;

            var stockRequests = this._stockRequestRepository.GetAllBasicByExpression(searchCriteria.Criteria());

            return stockRequests;
        }

        public IEnumerable<StockRequest> GetStockRequestsByOwner(string token)
        {

            if (string.IsNullOrEmpty(token.ToString()))
            {
                throw new InvalidResourceException("Invalid owner.");
            }

            var session = this._sessionRepository.GetOneByExpression(session => session.Token == new Guid(token));
            if (session == null)
            {
                throw new ResourceNotFoundException("Invalid owner.");
            }

            var userId = session.UserId;
            User user = this._employeeRepository.GetOneDetailByExpression(u => u.Id == userId);
            if (user == null)
            {
                throw new ResourceNotFoundException("Invalid user.");
            }

            var pharmacyId = user.Pharmacy.Id;
            var stockRequests = this._stockRequestRepository.GetAllBasicByExpression(s => s.Employee.Pharmacy.Id == pharmacyId);

            return stockRequests;

        }
    }
}

