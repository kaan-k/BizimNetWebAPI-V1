using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework; // For Context access
using Entities.Concrete.Sections;
using Entities.DTOs;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class SectionManager : ISectionService
    {
        private readonly IMapper _mapper;
        private readonly ISectionDal _sectionDal;
        private readonly BizimNetContext _context;

        public SectionManager(IMapper mapper, ISectionDal sectionDal, BizimNetContext context)
        {
            _mapper = mapper;
            _sectionDal = sectionDal;
            _context = context;
        }

        public IDataResult<Section> Add(SectionAddDto sectionAddDto)
        {
            var section = _mapper.Map<Section>(sectionAddDto);
            _sectionDal.Add(section);
            return new SuccessDataResult<Section>(section, "Bölüm başarıyla eklendi.");
        }

        public IResult Delete(int id)
        {
            var section = _sectionDal.Get(s => s.Id == id);
            if (section == null)
            {
                return new ErrorResult("Bölüm bulunamadı.");
            }

            // --- BUSINESS LOGIC ---
            // Prevent deleting a Section if it has tables inside it
            bool hasTables = _context.Tables.Any(t => t.SectionId == id);
            if (hasTables)
            {
                return new ErrorResult("Bu bölümde tanımlı masalar var. Önce masaları siliniz veya taşıyınız.");
            }

            _sectionDal.Delete(section);
            return new SuccessResult("Bölüm başarıyla silindi.");
        }

        public IDataResult<List<Section>> GetAll()
        {
            var sections = _sectionDal.GetAll();
            return new SuccessDataResult<List<Section>>(sections);
        }

        public IDataResult<SectionAddDto> GetById(int id)
        {
            var section = _sectionDal.Get(s => s.Id == id);
            if (section == null)
            {
                return new ErrorDataResult<SectionAddDto>("Bölüm bulunamadı.");
            }
            var dto = _mapper.Map<SectionAddDto>(section);
            return new SuccessDataResult<SectionAddDto>(dto);
        }

        public IResult Update(Section section)
        {
            _sectionDal.Update(section);
            return new SuccessResult("Bölüm güncellendi.");
        }
    }
}