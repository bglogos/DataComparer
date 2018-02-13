# Base64 data comparer

Table of content:

 1. API documentation
 2. Suggestions for improvement
 3. Assumptions made
 4. Developer notes

## 1. API documentation

__Setting the data that will be compared__

**Description**: Saves the left data item that will be compared with the right one from diff entry with the given `diffId`.\
**Method**: `POST`\
**Route**: `<host>/v1/diff/{diffId}/left`\
**Request content type**: `application/json`\
**Request body**:
```javascript
{
    Data: "Base64_encoded_data"
}
```
**Response:** Returns HTTP status 201 Created and the ID of the diff if the it is saved successfully.

---

**Description**: Saves the right data item that will be compared with the left one from diff entry with the given `diffId`.\
**Method**: `POST`\
**Route**: `<host>/v1/diff/{diffId}/right`\
**Request content type**: `application/json`\
**Request body**:
```javascript
{
    Data: "Base64_encoded_data"
}
```
**Response:** Returns HTTP status 201 Created and the ID of the diff if the it is saved successfully.

---

**Description**: Gets the result from comparing left and right data items from diff entry with the given `diffId`.\
**Method**: `GET`\
**Route**: `<host>/v1/diff/{diffId}`\
**Response:** The endpoint will produce JSON indicating the result type of the comparison. Possible types are:
* `LeftSideMissing` - only the right side data are provided.
* `RightSideMissing` - only the left side data are provided.
* `FullMatch` - left and right side data are identical.
* `SizeMatch` - left and right side data have identical binary length, but different content. When this result type is returned, additional information about the differences is also provided. This information contains the length of the difference in bits and zero-based offset indicating where the difference begins.
* `SizeMismatch` - left and right side data have different binary length.

_Note: If the provided `diffId` does not exists, the endpoint will produce HTTP status 404 Not found without additional object result._

### Examples
Saves base64 encoded data as right side of diff entry with ID 1

Request:

```http
POST /v1/diff/1/right HTTP/1.1
Host: localhost
Content-Type: application/json
```
```javascript
{
	Data: "gA7jOADw"
}	
```

Response:

```http
HTTP/1.1 201
status: 201
date: Sat, 17 Feb 2018 19:42:54 GMT
content-type: application/json; charset=utf-8
server: Kestrel
transfer-encoding: chunked
location: /v1/diff/1
api-supported-versions: 1
```
```
1
```

---

Gets the result from comparing data items with same sizes, but different content.

|Item side|Base64 string|Binary data               |
|---------|-------------|--------------------------|
|Left     |g64A         |10000011 10101110 00000000|
|Right    |4444         |11100011 10001110 00111000|

Request:

```http
GET /v1/diff/1 HTTP/1.1
accept: */*
host: localhost
accept-encoding: gzip, deflate
```

Response:

```http
HTTP/1.1 200
status: 200
date: Sat, 17 Feb 2018 16:57:32 GMT
content-type: application/json; charset=utf-8
server: Kestrel
transfer-encoding: chunked
api-supported-versions: 1
```
```javascript
{
    "type":"SizeMatch",
    "diffs":[
        {
            "offset":18,
            "length":3
        },
        {
            "offset":10,
            "length":1
        },
        {
            "offset":1,
            "length":2
        }
    ]
}
```

## 2. Suggestions for improvement
* Migrating to dedicated database server for better performance and scalability.
* Implementing error handling and error logging for improving the UX and aiding the maintenance.
* Adding user authentication mechanism for security if needed.
* Adding Swagger to the development environments for aiding development and QA testing.
* Adding input data validation for better UX.
* Use localization and return human readable labels from resource files for improving the UX.

## 3. Assumptions made
* If one of the compared items is not present, the application returns 200 OK HTTP status and result type indicating which side is missing. Alternatively the missing side could be considered as byte consisting only of zeros and compared with the other data item, which will cause detailed comparison with any other one-byte data. Another option is to produce different HTTP status code like 404.
* When trying to insert data on a side of a difference where other data already exist, the application will generate an error and return status code 400. If edit functionality is needed, a new PUT method should be created and used for updating the diff-ed data. Alternatively, the application may pull back from the REST principles and update an entry if it already exist.
* When trying to save empty or invalid base64 data, the application will generate an error and return status code 400. Alternatively, the data may be saved as they are sent and produce error when trying to compare them.
* Maximum request body size is set to 10MB. Allowing to save data larger than 255MB will require code improvements that will allow comparing them.

## 4. Developer notes
The generic repository class and interface are something I've been building and improving for some time across different projects. Their XML documentation style may be rewritten to match the rest of the project, but I assume it's not the focus of the assignment so I left them as they are. For the same reason, most of the repository methods are not used.