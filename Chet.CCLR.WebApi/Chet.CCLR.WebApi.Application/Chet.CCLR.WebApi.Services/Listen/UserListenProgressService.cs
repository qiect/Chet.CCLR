using AutoMapper;
using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.Domain.Listen;
using Chet.CCLR.WebApi.DTOs.Listen;
using Chet.CCLR.WebApi.DTOs.Classic;

namespace Chet.CCLR.WebApi.Services.Listen;

/// <summary>
/// 用户听读进度服务实现
/// </summary>
public class UserListenProgressService : IUserListenProgressService
{
    private readonly IUserListenProgressRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="repository">进度仓储</param>
    /// <param name="mapper">对象映射器</param>
    public UserListenProgressService(IUserListenProgressRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<ProgressResponseDto?> GetUserProgressAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default)
    {
        var progress = await _repository.GetByUserIdAndBookIdAsync(userId, bookId, cancellationToken);
        return _mapper.Map<ProgressResponseDto>(progress);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ProgressResponseDto>> GetUserAllProgressAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var progresses = await _repository.GetByUserIdAsync(userId, cancellationToken);
        return _mapper.Map<IEnumerable<ProgressResponseDto>>(progresses);
    }

    /// <inheritdoc />
    public async Task<ProgressResponseDto> UpdateUserProgressAsync(UpdateProgressRequestDto request, CancellationToken cancellationToken = default)
    {
        var progress = await _repository.GetByUserIdAndBookIdAsync(Guid.Parse(request.UserId), Guid.Parse(request.BookId), cancellationToken);
        if (progress != null)
        {
            // 更新现有进度
            _mapper.Map(request, progress);
            await _repository.UpdateAsync(progress, cancellationToken);
        }
        else
        {
            // 创建新进度
            progress = _mapper.Map<UserListenProgress>(request);
            progress.Id = Guid.CreateVersion7();
            progress.UserId = Guid.Parse(request.UserId);
            progress.BookId = Guid.Parse(request.BookId);
            progress.ChapterId = Guid.Parse(request.ChapterId);
            progress.SentenceId = Guid.Parse(request.SentenceId);
            await _repository.AddAsync(progress, cancellationToken);
        }

        return _mapper.Map<ProgressResponseDto>(progress);
    }

    /// <inheritdoc />
    public async Task<bool> ResetUserProgressAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default)
    {
        var progress = await _repository.GetByUserIdAndBookIdAsync(userId, bookId, cancellationToken);
        if (progress == null)
        {
            return false;
        }

        // 重置进度相关字段
        progress.ProgressSec = 0;
        progress.PlaySpeed = 1.0m;
        progress.AutoScroll = true;
        progress.ShowPinyin = true;
        progress.LastPositionPercent = 0.00m;
        progress.LastPlayTime = DateTime.Now;

        await _repository.UpdateAsync(progress, cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public async Task<SentenceResponseDto?> GetCurrentSentenceAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default)
    {
        var progress = await _repository.GetByUserIdAndBookIdAsync(userId, bookId, cancellationToken);
        if (progress == null)
        {
            return null;
        }

        // 这里需要获取句子详情，可能需要额外的服务或仓储
        // 暂时返回null，实际实现中需要整合句子服务
        return null;
    }

    /// <inheritdoc />
    public async Task<LearningStatsResponseDto> GetUserLearningStatsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var progresses = await _repository.GetByUserIdAsync(userId, cancellationToken);
        var progressesList = progresses.ToList();
        
        var totalBooks = progressesList.Count;
        var totalProgress = progressesList.Sum(p => p.LastPositionPercent);
        
        return new LearningStatsResponseDto
        {
            UserId = userId.ToString(),
            TotalBooks = totalBooks,
            AverageProgress = totalBooks > 0 ? totalProgress / totalBooks : 0,
            TotalHours = 0, // 需要从听读记录中计算
            CurrentStreak = 0, // 需要从听读记录中计算
            LongestStreak = 0, // 需要从听读记录中计算
            TotalStudyDays = 0, // 需要从听读记录中计算
            TotalSentences = 0, // 需要从听读记录中计算
            TotalFavorites = 0, // 需要从收藏记录中计算
            UpdatedAt = DateTime.Now
        };
    }

    /// <inheritdoc />
    public async Task<bool> DeleteUserProgressAsync(Guid userId, Guid bookId, CancellationToken cancellationToken = default)
    {
        var progress = await _repository.GetByUserIdAndBookIdAsync(userId, bookId, cancellationToken);
        if (progress == null)
        {
            return false;
        }

        await _repository.DeleteAsync(progress, cancellationToken);
        return true;
    }
}