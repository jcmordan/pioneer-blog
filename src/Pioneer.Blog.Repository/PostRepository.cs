﻿using System.Collections.Generic;
using Pioneer.Blog.DAL;
using Pioneer.Blog.DAL.Entites;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pioneer.Blog.Model;

namespace Pioneer.Blog.Repository
{
    public interface IPostRepository
    {
        int GetTotalNumberOfPosts();
        int GetTotalNumberOfPostsByCategory(string category);
        int GetTotalNumberOfPostByTag(string tag);
        PostEntity GetById(string id);
        IEnumerable<PostEntity> GetTop(int top);
        IEnumerable<PostEntity> GetAll();
        IEnumerable<PostEntity> GetAllPaged(int count, int page = 1);
        IEnumerable<PostEntity> GetAllByTagPaged(string tag, int count, int page = 1);
        IEnumerable<PostEntity> GetAllByCategoryPaged(string category, int count, int page = 1);
        IEnumerable<PostEntity> GetPostsBasedOnIdCollection(List<int> postIds);
        PostEntity GetPreviousBasedOnId(int id);
        PostEntity GetNextBasedOnId(int id);
        PostEntity Add(PostEntity map);
        void Update(Post post);
        void Remove(string url);
    }

    public class PostRepository : IPostRepository
    {
        private readonly BlogContext _blogContext;

        public PostRepository(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }

        /// <summary>
        /// Get a count of post
        /// </summary>
        /// <returns>Number of posts</returns>
        public int GetTotalNumberOfPosts()
        {
            return _blogContext.Posts.Count();
        }

        /// <summary>
        /// Get total number of posts by category
        /// </summary>
        /// <param name="category">category url</param>
        /// <returns>Collection of Posts</returns>
        public int GetTotalNumberOfPostsByCategory(string category)
        {
            return _blogContext
                    .Posts
                    .Where(x => x.Category.Url.ToLower() == category)
                    .Include(x => x.Category)
                    .Count();
        }

        /// <summary>
        /// Get total number of posts by tag
        /// </summary>
        /// <param name="tag">tag url</param>
        /// <returns>Collection of Posts</returns>
        public int GetTotalNumberOfPostByTag(string tag)
        {
            return _blogContext
                    .Posts
                    //.Where(x => x.Tags.Any(t => t.Url.ToLower() == tag))
                    // .Include(x => x.Tags)
                    .Count();
        }

        /// <summary>
        /// Get Post by id
        /// </summary>
        /// <param name="id">Id of post</param>
        /// <returns>Post</returns>
        public PostEntity GetById(string id)
        {
            return _blogContext
                    .Posts
                    .Include(x => x.Article)
                    .Include(x => x.Category)
                    .Include(x => x.PostTags)
                        .ThenInclude(i => i.Tag)
                    .First(x => x.Url == id);
        }

        /// <summary>
        /// Get top number of posts
        /// </summary>
        /// <param name="top">Top value to Take</param>
        /// <returns>Collection of posts</returns>
        public IEnumerable<PostEntity> GetTop(int top)
        {
            return _blogContext.Posts
                    .Where(x => x.Published)
                    .Include(x => x.Excerpt)
                    .Include(x => x.Category)
                    .Include(x => x.PostTags)
                        .ThenInclude(i => i.Tag)
                    .OrderByDescending(x => x.PostedOn)
                    .Take(top)
                    .ToList();
        }

        /// <summary>
        /// Return all Posts
        /// </summary>
        /// <returns>Collection of posts</returns>
        public IEnumerable<PostEntity> GetAll()
        {
            return _blogContext
                    .Posts
                    .Where(x => x.Published)
                    .Include(x => x.Excerpt)
                    .Include(x => x.Category)
                    .Include(x => x.PostTags)
                        .ThenInclude(i => i.Tag)
                    .OrderByDescending(x => x.PostedOn)
                    .ToList();
        }

        /// <summary>
        /// Get a collection of pages by skipping x and taking y
        /// </summary>
        /// <param name="count">The total number of posts you want to Take</param>
        /// <param name="page">The denomination of posts you want to skip. (page - 1) * count </param>
        /// <returns>Collections of posts</returns>
        public IEnumerable<PostEntity> GetAllPaged(int count, int page = 1)
        {
            return _blogContext
                    .Posts
                    .Where(x => x.Published)
                    .Include(x => x.Excerpt)
                    .Include(x => x.Category)
                    .Include(x => x.PostTags)
                        .ThenInclude(i => i.Tag)
                    .OrderByDescending(x => x.PostedOn)
                    .Skip((page - 1) * count)
                    .Take(count)
                    .ToList();
        }

        /// <summary>
        /// Get a collection of pages by skipping x and taking y, filtered by tag.
        /// </summary>
        /// <param name="tag">Filter by tag</param>
        /// <param name="count">The total number of posts you want to Take</param>
        /// <param name="page">The denomination of posts you want to skip. (page - 1) * count </param>
        /// <returns>Collections of posts</returns>
        public IEnumerable<PostEntity> GetAllByTagPaged(string tag, int count, int page = 1)
        {
            return _blogContext
                    .Posts
                    .Where(x => x.Published)
                    .Include(x => x.Excerpt)
                    .Include(x => x.Category)
                    .Include(x => x.PostTags)
                        .ThenInclude(i => i.Tag)
                    .Where(x => x.PostTags.Any(t => t.Tag.Url.ToLower() == tag))
                    .OrderByDescending(x => x.PostedOn)
                    .Skip((page - 1) * count)
                    .Take(count)
                    .ToList();
        }

        /// <summary>
        /// Get a collection of pages by skipping x and taking y, filtered by category.
        /// </summary>
        /// <param name="category">Filter by category</param>
        /// <param name="count">The total number of posts you want to Take</param>
        /// <param name="page">The denomination of posts you want to skip. (page - 1) * count </param>
        /// <returns>Collections of posts</returns>
        public IEnumerable<PostEntity> GetAllByCategoryPaged(string category, int count, int page = 1)
        {
            return _blogContext
                    .Posts
                    .Where(x => x.Published)
                    .Include(x => x.Excerpt)
                    .Include(x => x.Category)
                    .Include(x => x.PostTags)
                        .ThenInclude(i => i.Tag)
                    .Where(x => x.Category.Url.ToLower() == category)
                    .OrderByDescending(x => x.PostedOn)
                    .Skip((page - 1) * count)
                    .Take(count)
                    .ToList();
        }

        /// <summary>
        /// Get posts based on ids
        /// </summary>
        /// <param name="postIds">Collection of Ids</param>
        /// <returns>Collection of posts</returns>
        public IEnumerable<PostEntity> GetPostsBasedOnIdCollection(List<int> postIds)
        {
            return _blogContext
                    .Posts
                        .Where(t => postIds.Contains(t.PostId))
                    .Include(x => x.Category)
                    .OrderBy(d => postIds.IndexOf(d.PostId)).ToList()
                    .ToList();
        }

        /// <summary>
        /// Get Previous iteration of post based on id
        /// </summary>
        /// <param name="id">Post Id</param>
        /// <returns>Post Entity</returns>
        public PostEntity GetPreviousBasedOnId(int id)
        {
            return (from x in _blogContext.Posts where x.PostId < id orderby x.PostId descending select x).FirstOrDefault();
        }

        /// <summary>
        /// Get Next iteration of post based on id
        /// </summary>
        /// <param name="id">Post Id</param>
        /// <returns>Collection of Post Entities</returns>
        public PostEntity GetNextBasedOnId(int id)
        {
            return (from x in _blogContext.Posts where x.PostId > id orderby x.PostId ascending select x).FirstOrDefault();
        }

        /// <summary>
        /// Add new post
        /// </summary>
        /// <param name="post">Post model</param>
        /// <returns>New Post entity</returns>
        public PostEntity Add(PostEntity post)
        {
            _blogContext
              .Posts
              .Add(post);
            _blogContext.SaveChanges();

            return post;
        }

        /// <summary>
        /// Update post record
        /// </summary>
        /// <param name="post">Post model</param>
        public void Update(Post post)
        {
            var entity = _blogContext
                .Posts
                .FirstOrDefault(x => x.Url == post.Url);

            // Save logic 

            _blogContext.SaveChanges();
        }

        /// <summary>
        /// Remove post object
        /// </summary>
        /// <param name="url">Post URL</param>
        public void Remove(string url)
        {
            var entity = _blogContext
                .Posts
                .FirstOrDefault(x => x.Url == url);

            _blogContext.Posts.Remove(entity);
            _blogContext.SaveChanges();
        }
    }
}
