SELECT t.Id, t.Name, p.Id AS 'post Id', p.Title, p.Content, pt.TagId, pt.PostId
FROM Tag t
LEFT JOIN PostTag pt
ON t.id = pt.TagId
LEFT JOIN Post p
ON pt.PostId = p.Id;