CREATE TABLE "Tour" (
  "Tour_ID" SERIAL PRIMARY KEY,
  "Name" varchar(30) NOT NULL,
  "Description" varchar(30),
  "From" varchar(50),
  "To" varchar(50),
  "RouteType" int NOT NULL
);
