﻿//namespace BugTracker.Tests
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Net;
//    using System.Net.Http;
//    using System.Threading;

//    using BugTracker.Data.Models;
//    using BugTracker.Tests.Models;

//    using Microsoft.VisualStudio.TestTools.UnitTesting;

//    [TestClass]
//    public class CommentsTests
//    {
//        [TestMethod]
//        public void CreateBugWithComments_ListAllComments_ShouldListCommentsCorrectly()
//        {
//            // Arrange -> create a new bug with two comments
//            TestingEngine.CleanDatabase();

//            var bugTitle = "bug title";
//            var httpPostResponse = TestingEngine.SubmitBugHttpPost(bugTitle, null);
//            Assert.AreEqual(HttpStatusCode.Created, httpPostResponse.StatusCode);
//            var submittedBug = httpPostResponse.Content.ReadAsAsync<BugModel>().Result;

//            var httpPostResponseFirstComment =
//                TestingEngine.SubmitCommentHttpPost(submittedBug.Id, "Comment 1");
//            Assert.AreEqual(HttpStatusCode.OK, httpPostResponseFirstComment.StatusCode);
//            Thread.Sleep(2);

//            var httpPostResponseSecondComment =
//                TestingEngine.SubmitCommentHttpPost(submittedBug.Id, "Comment 2");
//            Assert.AreEqual(HttpStatusCode.OK, httpPostResponseSecondComment.StatusCode);

//            // Act -> find all comments
//            var httpResponse = TestingEngine.HttpClient.GetAsync("/api/comments").Result;

//            // Assert -> check the returned comments
//            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
//            var commentsFromService = httpResponse.Content.ReadAsAsync<List<CommentModel>>().Result;

//            Assert.AreEqual(2, commentsFromService.Count());

//            Assert.IsTrue(commentsFromService[0].Id > 0);
//            Assert.AreEqual("Comment 2", commentsFromService[0].Text);
//            Assert.AreEqual(null, commentsFromService[0].Author);
//            Assert.IsTrue(commentsFromService[0].DateCreated - DateTime.Now < TimeSpan.FromMinutes(1));
//            Assert.AreEqual(submittedBug.Id, commentsFromService[0].BugId);
//            Assert.AreEqual(bugTitle, commentsFromService[0].BugTitle);

//            Assert.IsTrue(commentsFromService[1].Id > 0);
//            Assert.AreEqual("Comment 1", commentsFromService[1].Text);
//            Assert.AreEqual(null, commentsFromService[1].Author);
//            Assert.IsTrue(commentsFromService[1].DateCreated - DateTime.Now < TimeSpan.FromMinutes(1));
//            Assert.AreEqual(submittedBug.Id, commentsFromService[1].BugId);
//            Assert.AreEqual(bugTitle, commentsFromService[1].BugTitle);
//        }
//    }
//}
