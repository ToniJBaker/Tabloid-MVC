SELECT p.Id, p.Title, t.Id, t.[Name], pt.PostId, pt.TagId
FROM Post p 
Left JOIN PostTag pt ON pt.PostId = p.Id
Left JOIN Tag t ON t.Id = pt.TagId
;

Select * From Tag;
