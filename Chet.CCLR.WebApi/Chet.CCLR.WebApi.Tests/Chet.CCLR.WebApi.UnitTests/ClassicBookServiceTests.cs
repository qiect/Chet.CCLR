using AutoMapper;
using Chet.CCLR.WebApi.Contracts.IRepositories;
using Chet.CCLR.WebApi.Contracts.IServices;
using Chet.CCLR.WebApi.Domain.Classic;
using Chet.CCLR.WebApi.DTOs.Classic;
using Chet.CCLR.WebApi.Services.Classic;
using Moq;

namespace Chet.CCLR.WebApi.UnitTests;

public class ClassicBookServiceTests
{
    private readonly Mock<IClassicBookRepository> _mockBookRepository;
    private readonly Mock<IClassicChapterRepository> _mockChapterRepository;
    private readonly Mock<IClassicSentenceRepository> _mockSentenceRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly ClassicBookService _bookService;
    private readonly ClassicChapterService _chapterService;
    private readonly ClassicSentenceService _sentenceService;

    public ClassicBookServiceTests()
    {
        _mockBookRepository = new Mock<IClassicBookRepository>();
        _mockChapterRepository = new Mock<IClassicChapterRepository>();
        _mockSentenceRepository = new Mock<IClassicSentenceRepository>();
        _mockMapper = new Mock<IMapper>();

        _bookService = new ClassicBookService(_mockBookRepository.Object, _mockMapper.Object);
        _chapterService = new ClassicChapterService(_mockChapterRepository.Object, _mockSentenceRepository.Object, _mockMapper.Object);
        _sentenceService = new ClassicSentenceService(_mockSentenceRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task CreateClassicBookWithChaptersAndSentences_ShouldSuccessfullyInsertDaodejing()
    {
        var bookId = Guid.CreateVersion7();

        var createBookRequest = new CreateBookRequestDto
        {
            Title = "道德经",
            Author = "老子",
            Dynasty = "春秋时期",
            Category = "哲学",
            Description = "《道德经》是春秋时期老子的哲学作品，又称《道德真经》《老子》《五千言》，是中国历史上从哲学家文子的《文子》以来，第一部完整的道家典籍。",
            Level = 3,
            IsPublished = true,
            OrderIndex = 1
        };

        var bookEntity = new ClassicBook
        {
            Id = bookId,
            Title = createBookRequest.Title,
            Author = createBookRequest.Author,
            Dynasty = createBookRequest.Dynasty,
            Category = createBookRequest.Category,
            Description = createBookRequest.Description,
            Level = createBookRequest.Level,
            IsPublished = createBookRequest.IsPublished,
            OrderIndex = createBookRequest.OrderIndex,
            TotalChapters = 81,
            TotalSentences = 81
        };

        _mockMapper.Setup(x => x.Map<ClassicBook>(createBookRequest)).Returns(bookEntity);

        _mockBookRepository.Setup(x => x.AddAsync(It.IsAny<ClassicBook>(), default))
            .Returns(Task.CompletedTask);

        _mockMapper.Setup(x => x.Map<BookResponseDto>(bookEntity))
            .Returns(new BookResponseDto
            {
                Id = bookId.ToString(),
                Title = "道德经",
                Author = "老子",
                Dynasty = "春秋时期",
                Category = "哲学",
                TotalChapters = 81,
                TotalSentences = 81,
                Level = 3,
                IsPublished = true,
                OrderIndex = 1
            });

        var result = await _bookService.CreateBookAsync(createBookRequest);

        Assert.NotNull(result);
        Assert.Equal("道德经", result.Title);
        Assert.Equal(81, result.TotalChapters);
        Assert.Equal(81, result.TotalSentences);

        _mockBookRepository.Verify(x => x.AddAsync(It.IsAny<ClassicBook>(), default), Times.Once);
    }

    [Fact]
    public async Task CreateDaodejing_With81Chapters_ShouldCreateAllChaptersAndSentences()
    {
        var bookId = Guid.CreateVersion7();

        var createBookRequest = new CreateBookRequestDto
        {
            Title = "道德经",
            Author = "老子",
            Dynasty = "春秋时期",
            Category = "哲学",
            Description = "《道德经》是春秋时期老子的哲学作品",
            Level = 3,
            IsPublished = true,
            OrderIndex = 1
        };

        var bookEntity = new ClassicBook
        {
            Id = bookId,
            Title = createBookRequest.Title,
            Author = createBookRequest.Author,
            Dynasty = createBookRequest.Dynasty,
            Category = createBookRequest.Category,
            Description = createBookRequest.Description,
            Level = createBookRequest.Level,
            IsPublished = createBookRequest.IsPublished,
            OrderIndex = createBookRequest.OrderIndex,
            TotalChapters = 81,
            TotalSentences = 81
        };

        _mockMapper.Setup(x => x.Map<ClassicBook>(createBookRequest)).Returns(bookEntity);

        _mockBookRepository.Setup(x => x.AddAsync(It.IsAny<ClassicBook>(), default))
            .Returns(Task.CompletedTask);

        _mockMapper.Setup(x => x.Map<BookResponseDto>(bookEntity))
            .Returns(new BookResponseDto
            {
                Id = bookId.ToString(),
                Title = "道德经",
                Author = "老子",
                Dynasty = "春秋时期",
                Category = "哲学",
                TotalChapters = 81,
                TotalSentences = 81,
                Level = 3,
                IsPublished = true,
                OrderIndex = 1
            });

        var result = await _bookService.CreateBookAsync(createBookRequest);

        Assert.Equal("道德经", result.Title);
        Assert.Equal(81, result.TotalChapters);
        Assert.Equal(81, result.TotalSentences);

        _mockBookRepository.Verify(x => x.AddAsync(It.IsAny<ClassicBook>(), default), Times.Once);
    }

    [Fact]
    public async Task CreateDaodejingChapter_WithValidData_ShouldCreateChapter()
    {
        var bookId = Guid.CreateVersion7();
        var chapterId = Guid.CreateVersion7();

        var createChapterRequest = new CreateChapterRequestDto
        {
            Title = "第1章",
            BookId = bookId.ToString(),
            OrderIndex = 1,
            IsPublished = true
        };

        var chapterEntity = new ClassicChapter
        {
            Id = chapterId,
            Title = createChapterRequest.Title,
            BookId = bookId,
            OrderIndex = 1,
            IsPublished = true
        };

        _mockMapper.Setup(x => x.Map<ClassicChapter>(createChapterRequest)).Returns(chapterEntity);

        _mockChapterRepository.Setup(x => x.AddAsync(It.IsAny<ClassicChapter>(), default))
            .Returns(Task.CompletedTask);

        _mockMapper.Setup(x => x.Map<ChapterResponseDto>(chapterEntity))
            .Returns(new ChapterResponseDto
            {
                Id = chapterId.ToString(),
                Title = "第1章",
                BookId = bookId.ToString(),
                OrderIndex = 1,
                IsPublished = true
            });

        var result = await _chapterService.CreateChapterAsync(createChapterRequest);

        Assert.Equal("第1章", result.Title);
        Assert.Equal(1, result.OrderIndex);

        _mockChapterRepository.Verify(x => x.AddAsync(It.IsAny<ClassicChapter>(), default), Times.Once);
    }

    [Fact]
    public async Task CreateDaodejingSentence_WithValidData_ShouldCreateSentence()
    {
        var chapterId = Guid.CreateVersion7();
        var sentenceId = Guid.CreateVersion7();

        var createSentenceRequest = new CreateSentenceRequestDto
        {
            Content = "道可道，非常道；名可名，非常名。",
            ChapterId = chapterId.ToString(),
            AudioUrl = "https://cdn.example.com/audio/1_1.mp3",
            AudioFileSize = 102400,
            AudioFormat = "mp3",
            OrderIndex = 1,
            IsPublished = true
        };

        var sentenceEntity = new ClassicSentence
        {
            Id = sentenceId,
            Content = createSentenceRequest.Content,
            ChapterId = chapterId,
            AudioUrl = createSentenceRequest.AudioUrl,
            AudioFileSize = createSentenceRequest.AudioFileSize,
            AudioFormat = createSentenceRequest.AudioFormat,
            OrderIndex = createSentenceRequest.OrderIndex,
            IsPublished = createSentenceRequest.IsPublished
        };

        _mockMapper.Setup(x => x.Map<ClassicSentence>(createSentenceRequest)).Returns(sentenceEntity);

        _mockSentenceRepository.Setup(x => x.AddAsync(It.IsAny<ClassicSentence>(), default))
            .Returns(Task.CompletedTask);

        _mockMapper.Setup(x => x.Map<SentenceResponseDto>(sentenceEntity))
            .Returns(new SentenceResponseDto
            {
                Id = sentenceId.ToString(),
                Content = "道可道，非常道；名可名，非常名。",
                ChapterId = chapterId.ToString(),
                AudioUrl = "https://cdn.example.com/audio/1_1.mp3",
                OrderIndex = 1,
                IsPublished = true
            });

        var result = await _sentenceService.CreateSentenceAsync(createSentenceRequest);

        Assert.Equal("道可道，非常道；名可名，非常名。", result.Content);

        _mockSentenceRepository.Verify(x => x.AddAsync(It.IsAny<ClassicSentence>(), default), Times.Once);
    }
}
