using System.ComponentModel.DataAnnotations;

namespace STech.Presentation.Api.Dtos.Employee.Api
{
    public class EmployeeFilterModel
    {
        public int FilterById { get; set; }

        [Range(0, short.MaxValue, ErrorMessage = "Please enter numarical value between {1} and {2}")]
        public string FilterByEmployeeAddress { get; set; }

    }


    public class EmployeeSortOrderModel
    {
        public bool OrderByFirstNameAsc { get; set; }
        public bool OrderByLastNameNoAsc { get; set; }

    }

    public class EmployeeSearchOptionsModel
    {
        public string SearchByName { get; set; }
        public short? SearchByBarnNo { get; set; }
        public string SearchByDescription { get; set; }
        public string SearchByAll { get; set; }

    }
}
