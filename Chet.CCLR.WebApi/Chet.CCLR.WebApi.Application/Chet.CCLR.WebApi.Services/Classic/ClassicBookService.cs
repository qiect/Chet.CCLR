using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.DTOs.Classic;
using Chet.CCLR.WebApi.Domain;
using Chet.CCLR.WebApi.Domain.Classic;
using AutoMapper;

namespace Chet.CCLR.WebApi.Services.Classic;

/// <summary>
/// 经典书籍服务实现
/// </summary>
public class ClassicBookService : IClassicBookService
{
    private readonly IClassicBookRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="repository">书籍仓储</param>
    /// <param name="mapper">对象映射器</param>
    public ClassicBookService(IClassicBookRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BookResponseDto>> GetAllBooksAsync(CancellationToken cancellationToken = default)
    {
        var books = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<BookResponseDto>>(books);
    }

    /// <inheritdoc />
    public async Task<BookResponseDto?> GetBookByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var book = await _repository.GetByIdAsync(id, cancellationToken);
        return _mapper.Map<BookResponseDto>(book);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BookResponseDto>> GetBooksByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {
        var books = await _repository.GetByCategoryAsync(category, cancellationToken);
        return _mapper.Map<IEnumerable<BookResponseDto>>(books);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BookResponseDto>> GetRecommendedBooksAsync(int limit = 10, CancellationToken cancellationToken = default)
    {
        var books = await _repository.GetRecommendedAsync(limit, cancellationToken);
        return _mapper.Map<IEnumerable<BookResponseDto>>(books);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BookResponseDto>> SearchBooksAsync(string keyword, CancellationToken cancellationToken = default)
    {
        var books = await _repository.SearchAsync(keyword, cancellationToken);
        return _mapper.Map<IEnumerable<BookResponseDto>>(books);
    }

    /// <inheritdoc />
    public async Task<BookResponseDto> CreateBookAsync(CreateBookRequestDto request, CancellationToken cancellationToken = default)
    {
        var book = _mapper.Map<ClassicBook>(request);
        book.Id = Guid.CreateVersion7();
        await _repository.AddAsync(book, cancellationToken);
        return _mapper.Map<BookResponseDto>(book);
    }

    /// <inheritdoc />
    public async Task<BookResponseDto?> UpdateBookAsync(Guid id, UpdateBookRequestDto request, CancellationToken cancellationToken = default)
    {
        var existingBook = await _repository.GetByIdAsync(id, cancellationToken);
        if (existingBook == null)
        {
            return null;
        }

        _mapper.Map(request, existingBook);
        await _repository.UpdateAsync(existingBook, cancellationToken);
        return _mapper.Map<BookResponseDto>(existingBook);
    }

    /// <inheritdoc />
    public async Task<bool> DeleteBookAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var exists = await _repository.ExistsAsync(id, cancellationToken);
        if (!exists)
        {
            return false;
        }

        var book = await _repository.GetByIdAsync(id, cancellationToken);
        if (book == null)
        {
            return false; // Should not happen if exists returned true
        }

        await _repository.DeleteAsync(book, cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public async Task<PagedResult<BookResponseDto>> GetPagedBooksAsync(int page, int size, CancellationToken cancellationToken = default)
    {
        var pagedResult = await _repository.GetPagedAsync(page, size, cancellationToken);
        var mappedItems = _mapper.Map<IEnumerable<BookResponseDto>>(pagedResult.Items);
        return new PagedResult<BookResponseDto>
        {
            Items = mappedItems,
            TotalCount = pagedResult.TotalCount,
            Page = pagedResult.Page,
            Size = pagedResult.Size
        };
    }
}