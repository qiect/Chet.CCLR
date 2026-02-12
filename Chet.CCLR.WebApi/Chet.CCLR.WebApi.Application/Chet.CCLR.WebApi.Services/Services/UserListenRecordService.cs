using AutoMapper;
using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.Domain.Listen;
using Chet.CCLR.WebApi.DTOs.Request.Listen;
using Chet.CCLR.WebApi.DTOs.Response.Listen;

namespace Chet.CCLR.WebApi.Services.Services;

/// <summary>
/// 用户听读记录服务实现
/// </summary>
public class UserListenRecordService : IUserListenRecordService
{
    private readonly IUserListenRecordRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="repository">记录仓储</param>
    /// <param name="mapper">对象映射器</param>
    public UserListenRecordService(IUserListenRecordRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<RecordResponseDto?> GetRecordByUserAndDateAsync(Guid userId, DateOnly date, CancellationToken cancellationToken = default)
    {
        var record = await _repository.GetByUserIdAndDateAsync(userId, date, cancellationToken);
        return _mapper.Map<RecordResponseDto>(record);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RecordResponseDto>> GetRecordsByUserAndDateRangeAsync(Guid userId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
    {
        var records = await _repository.GetByUserIdAndDateRangeAsync(userId, startDate, endDate, cancellationToken);
        return _mapper.Map<IEnumerable<RecordResponseDto>>(records);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<RecordResponseDto>> GetRecentRecordsAsync(Guid userId, int days = 7, CancellationToken cancellationToken = default)
    {
        var endDate = DateOnly.FromDateTime(DateTime.Now);
        var startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-days));
        var records = await _repository.GetByUserIdAndDateRangeAsync(userId, startDate, endDate, cancellationToken);
        return _mapper.Map<IEnumerable<RecordResponseDto>>(records);
    }

    /// <inheritdoc />
    public async Task<RecordResponseDto> CreateRecordAsync(CreateRecordRequestDto request, CancellationToken cancellationToken = default)
    {
        var record = _mapper.Map<UserListenRecord>(request);
        record.Id = Guid.CreateVersion7();
        record.UserId = Guid.Parse(request.UserId);
        record.ListenDate = request.ListenDate;
        record.DurationSec = request.DurationSeconds;
        record.SentenceIds = request.SentenceIds != null ? string.Join(",", request.SentenceIds) : null;
        record.BookId = Guid.Parse(request.BookId);
        record.ChapterId = Guid.Parse(request.ChapterId);
        await _repository.AddAsync(record, cancellationToken);
        return _mapper.Map<RecordResponseDto>(record);
    }

    /// <inheritdoc />
    public async Task<RecordResponseDto?> UpdateRecordAsync(Guid id, UpdateRecordRequestDto request, CancellationToken cancellationToken = default)
    {
        var existingRecord = await _repository.GetByIdAsync(id, cancellationToken);
        if (existingRecord == null)
        {
            return null;
        }

        _mapper.Map(request, existingRecord); // 将请求数据映射到现有实体
        await _repository.UpdateAsync(existingRecord, cancellationToken);
        return _mapper.Map<RecordResponseDto>(existingRecord);
    }

    /// <inheritdoc />
    public async Task<int> GetConsecutiveListenDaysAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        // 实际实现中需要复杂逻辑来计算连续听读天数
        // 暂时返回0，实际实现中需要从记录中计算
        var records = await _repository.GetByUserIdAsync(userId, cancellationToken);
        return 0;
    }

    /// <inheritdoc />
    public async Task<int> GetTotalListenDaysAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var records = await _repository.GetByUserIdAsync(userId, cancellationToken);
        return records.Count();
    }

    /// <inheritdoc />
    public async Task<bool> DeleteRecordAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var record = await _repository.GetByIdAsync(id, cancellationToken);
        if (record == null)
        {
            return false;
        }

        await _repository.DeleteAsync(record, cancellationToken);
        return true;
    }
}