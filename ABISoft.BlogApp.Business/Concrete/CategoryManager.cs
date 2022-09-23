using ABISoft.BlogApp.Business.Abstract;
using ABISoft.BlogApp.DataAccess.Abstract.UnitOfWork;
using ABISoft.BlogApp.Entities.Concrete;
using ABISoft.BlogApp.Entities.Dtos;
using ABISoft.BlogApp.Shared.Utilities.Results.Abstract;
using ABISoft.BlogApp.Shared.Utilities.Results.ComplexTypes;
using ABISoft.BlogApp.Shared.Utilities.Results.Concrete;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABISoft.BlogApp.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper; 
        public CategoryManager(IUnitOfWork uof, IMapper mapper)
        {
            _uow = uof;
            _mapper = mapper;
        }
        public async Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName)
        {
            var category = _mapper.Map<Category>(categoryAddDto);
            category.CreatedByName = createdByName;
            category.ModifiedByName = createdByName;
            var categoryFromAdd = await _uow.CategoryRepository.AddAsync(category);
            await _uow.SaveChangesAsync();
            return new DataResult<CategoryDto>(ResultStatus.Success, $"{categoryAddDto.Name} adlı kategori başarıyla eklenmiştir", new CategoryDto()
            {
                ResultStatus = ResultStatus.Success,
                Message = $"{categoryAddDto.Name} adlı kategori başarıyla eklenmiştir",
                Category = categoryFromAdd,
            }); 
        }
        public async Task<IDataResult<CategoryDto>> SoftDeleteAsync(int categoryId, string modifiedByName)
        {
            var softDeletedCategory = await _uow.CategoryRepository.GetAsync(x => x.Id == categoryId);
            if(softDeletedCategory != null)
            {
                softDeletedCategory.IsDeleted = true;
                softDeletedCategory.ModifiedByName = modifiedByName;
                softDeletedCategory.ModifiedDate = DateTime.Now;
                var deletedCategory = await _uow.CategoryRepository.UpdateAsync(softDeletedCategory);
                await _uow.SaveChangesAsync();
                return new DataResult<CategoryDto>(ResultStatus.Success, $"{deletedCategory.Name} adlı kategori başarıyla silinmiştir.", new CategoryDto()
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"{deletedCategory.Name} adlı kategori başarıyla silinmiştir.",
                    Category = deletedCategory
                });
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, $"Böyle bir kategori bulunamadı.", new CategoryDto()
            {
                ResultStatus = ResultStatus.Error,
                Message = $"Böyle bir kategori bulunamadı.",
                Category = null
            });
        }
        public async Task<IDataResult<CategoryListDto>> GetAllAsync()
        {
            //var categoryList = await _uow.CategoryRepository.GetAllWithoutIncludesAsync();
            var categoryList = await _uow.CategoryRepository.GetAllAsync(null, x => x.Articles);
  
            if (categoryList.Count > -1) 
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto() { 
                     Categories = categoryList,
                     ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Bir hata oluştu", new CategoryListDto() { 
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message = "Bir hata oluştu"
            });
        }
        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAsync()
        {
            var categoryList = await _uow.CategoryRepository.GetAllAsync(x => !x.IsDeleted, x => x.Articles);
            if (categoryList.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto()
                {
                    Categories = categoryList,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, "Bir hata oluştu", new CategoryListDto()
            {
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message = "Bir hata oluştu"
            });
        }
        public async Task<IDataResult<CategoryDto>> GetAsync(int categoryId)
        {
            var category = await _uow.CategoryRepository.GetAsync(x => x.Id == categoryId, x => x.Articles);
            if(category != null)
            {
                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto()
                {
                    Category = category,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, "Böyle bir kategori bulunamadı.", new CategoryDto()
            {
                ResultStatus = ResultStatus.Error,
                Message = "Böyle bir kategori bulunamadı.",
                Category = null
            });
        }
        public async Task<IResult> HardDeleteAsync(int categoryId)
        {
            var hardDeletedCategory = await _uow.CategoryRepository.GetAsync(x => x.Id == categoryId);
            if(hardDeletedCategory != null)
            {
                await _uow.CategoryRepository.DeleteAsync(hardDeletedCategory);
                await _uow.SaveChangesAsync();
                return new Result(ResultStatus.Success, $"{hardDeletedCategory.Name} adlı kategori veritabanından başarıyla silinmiştir");
            }
            return new Result(ResultStatus.Error, "Silme işlemi gerçekleştirilirken bir hatayla karşılaşıldı.");
        }
        public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var previousCategory = await _uow.CategoryRepository.GetAsync(x => x.Id == categoryUpdateDto.Id);
            var updatedCategory = _mapper.Map<CategoryUpdateDto, Category>(categoryUpdateDto, previousCategory); //previousCategory and categoryUpdateDto were mixed!!
            updatedCategory.ModifiedByName = modifiedByName;
            var categoryFromUpdate = await _uow.CategoryRepository.UpdateAsync(updatedCategory);
            await _uow.SaveChangesAsync();
            return new DataResult<CategoryDto>(ResultStatus.Success, $"{updatedCategory.Name} adlı kategori başarıyla güncellenmiştir.", new CategoryDto()
            {
                ResultStatus = ResultStatus.Success,
                Message = $"{updatedCategory.Name} adlı kategori başarıyla güncellenmiştir.",
                Category = categoryFromUpdate
            });
        }
        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var categories = await _uow.CategoryRepository.GetAllAsync(x => !x.IsDeleted && x.IsActive, x => x.Articles);
            if(categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto()
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, "", null);
        }
        public async Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDtoAsync(int categoryId)
        {
            var isFound = await _uow.CategoryRepository.AnyAsync(x => x.Id == categoryId);
            if (isFound)
            {
                var category = await _uow.CategoryRepository.GetAsync(x => x.Id == categoryId); //No need for includes
                var categoryUpdateDto = _mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);
            }
            return new DataResult<CategoryUpdateDto>(ResultStatus.Error, "Böyle bir kategori bulunamadı.", null);
        }
    }
}
