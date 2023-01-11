using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO.Parameters
{
    public abstract class PaginationDTO
    {
        private const int _maxItemsPerPage = 20;
        private int itemsPerPage;

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
        [Required]
        [Range(1, 20)]
        public int PageSize
        {
            get => itemsPerPage;
            set => itemsPerPage = value > _maxItemsPerPage ? _maxItemsPerPage : value;
        }
    }
}
