# Database Schema

This document describes the PostgreSQL database structure used in **BZTEHUB**, including user authentication, university members, academic ranks, source links, and student curricula.

---

## 1. Users

Stores user account information, authentication credentials, and refresh token data.

**Table: `users`**

```sql
CREATE TABLE users (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    email VARCHAR(254) UNIQUE NOT NULL,
    password VARCHAR(256) NOT NULL,
    refreshtoken VARCHAR(256) NOT NULL,
    expires_refreshtoken TIMESTAMPTZ NOT NULL,
    created_at TIMESTAMPTZ DEFAULT NOW()
);
```

### Columns

| Column | Data Type | Constraints | Description |
|---|---|---|---|
| `id` | BIGINT | PRIMARY KEY, IDENTITY | Unique user identifier |
| `email` | VARCHAR(254) | UNIQUE, NOT NULL | User's email address |
| `password` | VARCHAR(256) | NOT NULL | Hashed user password |
| `refreshtoken` | VARCHAR(256) | NOT NULL | Hashed refresh token |
| `expires_refreshtoken` | TIMESTAMPTZ | NOT NULL | Refresh token expiration time |
| `created_at` | TIMESTAMPTZ | DEFAULT NOW() | Account creation timestamp |

### Security Notes

- Passwords are hashed using **BCrypt** before being stored.
- Refresh tokens are generated using a cryptographically secure random number generator.
- Refresh tokens are hashed using **SHA-512** before being stored.
- Refresh tokens are invalidated when the user logs out.
- All timestamps are stored using PostgreSQL `TIMESTAMPTZ`.

---

## 2. University Members

Stores information about university staff, professors, and other academic personnel.

**Table: `university_members`**

```sql
CREATE TABLE university_members (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    um_name VARCHAR(254) NOT NULL,
    um_phone VARCHAR(16) UNIQUE,
    um_tel_id VARCHAR(100) UNIQUE,
    um_bale_id VARCHAR(100) UNIQUE,
    um_email VARCHAR(254) UNIQUE,
    um_sex VARCHAR(50) DEFAULT 'MAN',
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
```

### Columns

| Column | Data Type | Constraints | Description |
|---|---|---|---|
| `id` | BIGINT | PRIMARY KEY, IDENTITY | Unique member identifier |
| `um_name` | VARCHAR(254) | NOT NULL | Member's full name |
| `um_phone` | VARCHAR(16) | UNIQUE | Mobile phone number |
| `um_tel_id` | VARCHAR(100) | UNIQUE | Telegram identifier |
| `um_bale_id` | VARCHAR(100) | UNIQUE | Bale identifier |
| `um_email` | VARCHAR(254) | UNIQUE | Member's email address |
| `um_sex` | VARCHAR(50) | DEFAULT 'MAN' | Member's gender |
| `created_at` | TIMESTAMPTZ | NOT NULL, DEFAULT NOW() | Record creation timestamp |

---

## 3. University Members Academic Ranks

Stores the academic ranks associated with university members. Each member can have multiple academic ranks, while duplicate ranks for the same member are prevented.

**Table: `university_members_academic_rank`**

```sql
CREATE TABLE university_members_academic_rank (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    um_member_id BIGINT NOT NULL,
    um_member_academic_rank VARCHAR(100) NOT NULL,

    CONSTRAINT um_member_rank
        FOREIGN KEY (um_member_id)
        REFERENCES university_members(id)
        ON DELETE CASCADE,

    CONSTRAINT um_member_rank_unique
        UNIQUE (um_member_id, um_member_academic_rank)
);
```

### Columns

| Column | Data Type | Constraints | Description |
|---|---|---|---|
| `id` | BIGINT | PRIMARY KEY, IDENTITY | Unique rank record identifier |
| `um_member_id` | BIGINT | FOREIGN KEY, NOT NULL | References the associated university member |
| `um_member_academic_rank` | VARCHAR(100) | NOT NULL | Academic rank of the member |

**Relationship:** One university member can have multiple academic rank records.

**Referential Integrity:** Deleting a university member automatically deletes their associated academic rank records through `ON DELETE CASCADE`.

---

## 4. Source Links

Stores external resources and useful links, such as university channels, educational materials, and other information sources.

**Table: `source_links`**

```sql
CREATE TABLE source_links (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    source_with_name VARCHAR(200) UNIQUE NOT NULL,
    source_link VARCHAR(2048) UNIQUE NOT NULL,
    description TEXT
);
```

### Columns

| Column | Data Type | Constraints | Description |
|---|---|---|---|
| `id` | BIGINT | PRIMARY KEY, IDENTITY | Unique source identifier |
| `source_with_name` | VARCHAR(200) | UNIQUE, NOT NULL | Display name of the source |
| `source_link` | VARCHAR(2048) | UNIQUE, NOT NULL | URL of the external resource |
| `description` | TEXT | NULLABLE | Additional information about the source |

---

## 5. Curriculum

Stores each user's weekly class schedule, including lesson names, class days, starting times, and classroom numbers.

**Table: `curriculum`**

```sql
CREATE TABLE curriculum (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    userid BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    lesson_name VARCHAR(100) NOT NULL,
    day_of_class SMALLINT NOT NULL
        CHECK (day_of_class BETWEEN 0 AND 6),
    start_class TIME NOT NULL,
    class_loc SMALLINT DEFAULT 600,
    created_at TIMESTAMPTZ DEFAULT NOW()
);
```

### Columns

| Column | Data Type | Constraints | Description |
|---|---|---|---|
| `id` | BIGINT | PRIMARY KEY, IDENTITY | Unique class record identifier |
| `userid` | BIGINT | FOREIGN KEY, NOT NULL | References the user who owns the schedule |
| `lesson_name` | VARCHAR(100) | NOT NULL | Name of the lesson |
| `day_of_class` | SMALLINT | NOT NULL, CHECK (0–6) | Day of the week |
| `start_class` | TIME | NOT NULL | Class starting time |
| `class_loc` | SMALLINT | DEFAULT 600 | Classroom number |
| `created_at` | TIMESTAMPTZ | DEFAULT NOW() | Record creation timestamp |

### Schedule Notes

- Each class is associated with a specific user.
- Days of the week are represented by values from `0` to `6`.
- Class duration is assumed to be **2 hours** and the finishing time is calculated by the backend.
- `start_class` uses PostgreSQL `TIME` because the weekly schedule does not require a specific calendar date.
- Deleting a user automatically deletes all of their curriculum records through `ON DELETE CASCADE`.

---

## 6. Unique Curriculum

Ensures that each user can have only one registered weekly curriculum.

**Table: `unique_curriculumn`**

```sql
CREATE TABLE unique_curriculumn (
    id BIGINT GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    user_id BIGINT UNIQUE NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
);
```

### Columns

| Column | Data Type | Constraints | Description |
|---|---|---|---|
| `id` | BIGINT | PRIMARY KEY, IDENTITY | Unique curriculum identifier |
| `user_id` | BIGINT | UNIQUE, FOREIGN KEY, NOT NULL | References the user who owns the curriculum |
| `created_at` | TIMESTAMPTZ | NOT NULL, DEFAULT NOW() | Record creation timestamp |

### Design Notes

- Each user can have only one record in this table.
- The `UNIQUE` constraint on `user_id` prevents duplicate curriculum registrations.
- The actual class records are stored separately in the `curriculum` table.
- When the last class is deleted, the backend also removes the corresponding record from `unique_curriculumn`, allowing the user to register a new schedule.

---

## Database Relationships

| Parent Table | Child Table | Relationship | Delete Behavior |
|---|---|---|---|
| `university_members` | `university_members_academic_rank` | One-to-Many | CASCADE |
| `users` | `curriculum` | One-to-Many | CASCADE |
| `users` | `unique_curriculumn` | One-to-One | CASCADE |

---

**Database Engine:** PostgreSQL  
**Data Access:** Npgsql  
**Backend Framework:** ASP.NET Core