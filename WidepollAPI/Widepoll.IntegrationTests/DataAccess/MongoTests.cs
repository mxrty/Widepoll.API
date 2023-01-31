using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Entities;
using WidepollAPI.DataAccess;
using WidepollAPI.Models;

namespace Widepoll.IntegrationTests.DataAccess;

public class MongoTests
{
    private MongoStore _mongo;

    private const string UserName = "Abraham Lincolm";

    [OneTimeSetUp]
    public async Task Setup()
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<MongoTests>().Build();
        var connectionString = configuration.GetValue<string>("Mongo:ConnectionString");
        await DB.InitAsync("test", MongoClientSettings.FromConnectionString(connectionString));
        _mongo = new MongoStore();
    }

    [OneTimeTearDown]
    public async Task CleanUp()
    {
        await DB.DropCollectionAsync<User>();
        await DB.DropCollectionAsync<Comment>();
        await DB.DropCollectionAsync<Like>();
        await DB.DropCollectionAsync<Post>();
        await DB.DropCollectionAsync<Statement>();
    }

    [Test]
    public async Task InsertAndGetUser()
    {
        var testUserName = "testuser";
        var createdUser = await GenerateUser(testUserName);

        Assert.That(createdUser, Is.Not.Null);
        Assert.That(createdUser.Name, Is.EqualTo(testUserName));

        var fetchedUser = await _mongo.GetByIdAsync<User>(createdUser.ID);
        Assert.That(fetchedUser, Is.EqualTo(createdUser));
    }

    [Test]
    public async Task InsertAndGetComment()
    {
        var user = await GenerateUser();
        var comment = new Comment
        {
            Author = user,
            Body = "Lorem Ipsum"
        };
        var createdComment = await _mongo.InsertAsync(comment);

        Assert.That(createdComment, Is.Not.Null);
        Assert.That(createdComment.Body, Is.EqualTo(comment.Body));
        Assert.That(createdComment.Author, Is.EqualTo(comment.Author));

        var fetchedComment = await _mongo.GetByIdAsync<Comment>(createdComment.ID);
        Assert.That(fetchedComment, Is.EqualTo(createdComment));
    }

    [Test]
    public async Task InsertAndGetLike()
    {
        var user = await GenerateUser();
        var like = new Like
        {
            Author = user,
            PostId = "123",
            CommentId = "456"
        };
        var createdLike = await _mongo.InsertAsync(like);

        Assert.That(createdLike, Is.Not.Null);
        Assert.That(createdLike.PostId, Is.EqualTo(like.PostId));
        Assert.That(createdLike.CommentId, Is.EqualTo(like.CommentId));
        Assert.That(createdLike.Author, Is.EqualTo(like.Author));

        var fetchedLike = await _mongo.GetByIdAsync<Like>(createdLike.ID);
        Assert.That(fetchedLike, Is.EqualTo(createdLike));
    }

    [Test]
    public async Task InsertAndGetStatement()
    {
        var user = await GenerateUser();
        var statement = new Statement { Author = user, Left = "Apples", Link = "are", Right = "healthy" };
        var createdStatement = await _mongo.InsertAsync(statement);

        Assert.That(createdStatement, Is.Not.Null);
        Assert.That(createdStatement.Author, Is.EqualTo(statement.Author));
        Assert.That(createdStatement.Left, Is.EqualTo(statement.Left));
        Assert.That(createdStatement.Link, Is.EqualTo(statement.Link));
        Assert.That(createdStatement.Right, Is.EqualTo(statement.Right));

        var fetchedPost = await _mongo.GetByIdAsync<Statement>(createdStatement.ID);
        Assert.That(fetchedPost, Is.EqualTo(createdStatement));
    }

    [Test]
    public async Task InsertAndGetPost()
    {
        var user = await GenerateUser();
        var statement = await GenerateStatement(user);
        var post = new Post
        {
            Author = user,
            Statement = statement
        };
        var createdPost = await _mongo.InsertAsync(post);

        Assert.That(createdPost, Is.Not.Null);
        Assert.That(createdPost.Statement, Is.EqualTo(post.Statement));
        Assert.That(createdPost.Author, Is.EqualTo(post.Author));

        var fetchedPost = await _mongo.GetByIdAsync<Post>(createdPost.ID);
        Assert.That(fetchedPost, Is.EqualTo(createdPost));
    }

    [Test]
    public async Task AddCommentIdToParentComment()
    {
        var user = await GenerateUser();
        var parentComment = await GenerateComment(user);
        var comment1 = await GenerateComment(user);
        var comment2 = await GenerateComment(user);

        await _mongo.AddCommentIdToParentCommentAsync(parentComment, comment1.ID);
        await _mongo.AddCommentIdToParentCommentAsync(parentComment, comment2.ID);
        var fetchedComment = await _mongo.GetByIdAsync<Comment>(parentComment.ID);

        Assert.That(fetchedComment, Is.Not.Null);
        Assert.That(fetchedComment.ReplyIds.Count, Is.EqualTo(2));
        Assert.That(fetchedComment.ReplyIds.Contains(comment1.ID), Is.True);
        Assert.That(fetchedComment.ReplyIds.Contains(comment2.ID), Is.True);
    }

    [Test]
    public async Task AddLikeToComment()
    {
        var user = await GenerateUser();
        var parentComment = await GenerateComment(user);
        var like1 = await GenerateLike(user, parentCommentId: parentComment.ID);
        var like2 = await GenerateLike(user, parentCommentId: parentComment.ID);

        await _mongo.AddLikeToCommentAsync(parentComment, like1);
        await _mongo.AddLikeToCommentAsync(parentComment, like2);
        var fetchedComment = await _mongo.GetByIdAsync<Comment>(parentComment.ID);

        Assert.That(fetchedComment, Is.Not.Null);
        Assert.That(fetchedComment.Likes.Count, Is.EqualTo(2));
        Assert.That(fetchedComment.Likes.SingleOrDefault(l => l.ID == like1.ID), Is.EqualTo(like1));
        Assert.That(fetchedComment.Likes.SingleOrDefault(l => l.ID == like2.ID), Is.EqualTo(like2));
    }

    [Test]
    public async Task AddLikeToPost()
    {
        var user = await GenerateUser();
        var parentPost = await GeneratePost(user);
        var like1 = await GenerateLike(user, parentPostId: parentPost.ID);
        var like2 = await GenerateLike(user, parentPostId: parentPost.ID);

        await _mongo.AddLikeToPostAsync(parentPost, like1);
        await _mongo.AddLikeToPostAsync(parentPost, like2);
        var fetchedPost = await _mongo.GetByIdAsync<Post>(parentPost.ID);

        Assert.That(fetchedPost, Is.Not.Null);
        Assert.That(fetchedPost.Likes.Count, Is.EqualTo(2));
        Assert.That(fetchedPost.Likes.SingleOrDefault(l => l.ID == like1.ID), Is.EqualTo(like1));
        Assert.That(fetchedPost.Likes.SingleOrDefault(l => l.ID == like2.ID), Is.EqualTo(like2));
    }

    [Test]
    public async Task GetCommentsByParentPostId()
    {
        var user = await GenerateUser();
        var parentPost = await GeneratePost(user);
        var otherPost = await GeneratePost(user);
        var comment1 = await GenerateComment(user, parentPost.ID);
        var comment2 = await GenerateComment(user, parentPost.ID);
        var comment3 = await GenerateComment(user, otherPost.ID);

        var childComments = await _mongo.GetCommentsByParentPostIdAsync(parentPost.ID);

        Assert.That(childComments.Count, Is.EqualTo(2));
        Assert.That(childComments.SingleOrDefault(c => c.ID == comment1.ID), Is.EqualTo(comment1));
        Assert.That(childComments.SingleOrDefault(c => c.ID == comment2.ID), Is.EqualTo(comment2));
        Assert.That(childComments.SingleOrDefault(c => c.ID == comment3.ID), Is.Null);
    }

    [Test]
    public async Task GetRecentPosts()
    {
        var user = await GenerateUser();
        var post1 = await GeneratePost(user);
        var post2 = await GeneratePost(user);
        var post3 = await GeneratePost(user);

        var recentPosts = _mongo.GetRecentPosts(2);

        Assert.That(recentPosts.Count, Is.EqualTo(2));
        Assert.That(recentPosts.SingleOrDefault(x => x.ID == post1.ID), Is.Null);
        Assert.That(recentPosts.SingleOrDefault(x => x.ID == post2.ID), Is.EqualTo(post2));
        Assert.That(recentPosts.SingleOrDefault(x => x.ID == post3.ID), Is.EqualTo(post3));
    }

    private async Task<User> GenerateUser(string name = UserName)
    {
        var user = new User { Name = name };
        return await _mongo.InsertAsync(user);
    }

    private async Task<Statement> GenerateStatement(User user)
    {
        var statement = new Statement { Author = user, Left = "Apples", Link = "are", Right = "healthy" };
        return await _mongo.InsertAsync(statement);
    }

    private async Task<Comment> GenerateComment(User user, string parentPostId = "123")
    {
        var comment = new Comment { Author = user, Body = "Lorem Ipsum", PostId = parentPostId};
        return await _mongo.InsertAsync(comment);
    }

    private async Task<Like> GenerateLike(User user, string parentPostId = "123", string parentCommentId = "456")
    {
        var like = new Like
        {
            Author = user,
            PostId = parentPostId,
            CommentId = parentCommentId
        };
        return await _mongo.InsertAsync(like);
    }

    private async Task<Post> GeneratePost(User user, Statement statement = null)
    {
        var post = new Post { Author = user, Statement = statement};
        return await _mongo.InsertAsync(post);
    }
}

