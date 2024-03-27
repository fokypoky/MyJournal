CREATE TABLE Contacts(
    id SERIAL NOT NULL PRIMARY KEY,
    surname VARCHAR(50) NOT NULL,
    name VARCHAR(50) NOT NULL,
    midname VARCHAR(50),
    phone_number VARCHAR(15) NOT NULL UNIQUE,
    email VARCHAR(100) UNIQUE,
    password VARCHAR(20) NOT NULL,
    sex VARCHAR(1) NOT NULL
);
CREATE TABLE Employees(
    id SERIAL NOT NULL PRIMARY KEY,
    contacts_id INT NOT NULL,
    FOREIGN KEY (contacts_id) REFERENCES Contacts(id)
);
CREATE TABLE Auditories(
    id SERIAL NOT NULL PRIMARY KEY,
    auditory_number VARCHAR(10) NOT NULL UNIQUE
);
CREATE TABLE Classes(
    id SERIAL NOT NULL PRIMARY KEY,
    class_number VARCHAR(5) NOT NULL UNIQUE,
    leader_id INT,
    auditory_id INT,
    FOREIGN KEY (leader_id) REFERENCES Employees(id),
    FOREIGN KEY (auditory_id) REFERENCES Auditories(id)
);
CREATE TABLE Students(
    id SERIAL NOT NULL PRIMARY KEY,
    class_id INT NOT NULL,
    contacts_id INT NOT NULL,
    FOREIGN KEY (class_id) REFERENCES Classes(id),
    FOREIGN KEY (contacts_id) REFERENCES Contacts(id)
);
CREATE TABLE Parents(
    id SERIAL NOT NULL PRIMARY KEY,
    contacts_id INT NOT NULL,
    FOREIGN KEY (contacts_id) REFERENCES Contacts(id)
);
CREATE TABLE Parent_student(
    id SERIAL NOT NULL PRIMARY KEY,
    parent_id INT NOT NULL,
    student_id INT NOT NULL,
    FOREIGN KEY (parent_id) REFERENCES Parents(id),
    FOREIGN KEY (student_id) REFERENCES Students(id)
);
CREATE TABLE Subjects(
    id SERIAL NOT NULL PRIMARY KEY,
    subject_title VARCHAR(50) NOT NULL UNIQUE
);
CREATE TABLE Employee_Subject(
    id SERIAL NOT NULL PRIMARY KEY,
    employee_id INT NOT NULL,
    subject_id INT NOT NULL,
    FOREIGN KEY (employee_id) REFERENCES Employees(id),
    FOREIGN KEY (subject_id) REFERENCES Subjects(id)
);
CREATE TABLE Tasks(
    id SERIAL NOT NULL PRIMARY KEY,
    subject_id INT NOT NULL,
    class_id INT NOT NULL,
    task_text VARCHAR(200) NOT NULL,
    teacher_id INT NOT NULL,
    FOREIGN KEY (subject_id) REFERENCES Subjects(id),
    FOREIGN KEY (class_id) REFERENCES Classes(id),
    FOREIGN KEY (teacher_id) REFERENCES Employees(id)
);
CREATE TABLE Marks(
    id SERIAL NOT NULL PRIMARY KEY,
    student_id INT NOT NULL,
    mark INT NOT NULL,
    mark_date TIMESTAMP NOT NULL,
    teacher_id INT,
    task_id INT,
    FOREIGN KEY (student_id) REFERENCES Students(id),
    FOREIGN KEY (teacher_id) REFERENCES Employees(id),
    FOREIGN KEY (task_id) REFERENCES Tasks(id)
);
CREATE TABLE Timetable(
    id SERIAL NOT NULL PRIMARY KEY,
    day_of_week INT NOT NULL,
    lesson_time TIME NOT NULL,
    class_id INT NOT NULL,
    subject_id INT NOT NULL,
    auditory_id INT NOT NULL,
    teacher_id INT NOT NULL,
    FOREIGN KEY (class_id) REFERENCES Classes(id),
    FOREIGN KEY (subject_id) REFERENCES Subjects(id),
    FOREIGN KEY (auditory_id) REFERENCES Auditories(id),
    FOREIGN KEY (teacher_id) REFERENCES Employees(id)
);
-- добавляем информацию
    -- предметы
INSERT INTO Subjects(subject_title)
VALUES ('Математика'),('Физика'),
       ('Информатика'),('Химия'),
       ('Биология'),('Основы Безопасности Жизнедеятельности'),
       ('Физическая культура'), ('Русский язык'),
       ('Иностранный язык'),('Литература'),
       ('История'),('Обществознание'),
       ('Изобразительное искусство'),('Черчение'),
       ('Технология'),('География'),
       ('Истоки'),('Астрономия');
    -- аудитории
INSERT INTO Auditories(auditory_number)
VALUES ('32-А'),('45'),
       ('37'),('10-Б'),
       ('30'),('29');
    -- классы
INSERT INTO Contacts(surname, name, midname, phone_number, email, password, sex)
VALUES ('admin', 'admin', NULL, 'admin', 'admin', 'admin', 'М'),
       ('Шнюк', 'Фулиона', 'Сергеевна', '+79108883288', 'shnyuk@gmail.com', 'shnyuk', 'Ж'),
       ('Фулько', 'Сергей', 'Викторович', '+79157321900', 'fulya@gmail.com', 'fulya', 'М'),
       ('Жмышенко', 'Валерий', 'Альбертович', '+79109001488', 'jmih@gmail.com', 'jmih', 'М'),
       ('Свинья', 'Надежда', 'Александровна', '+79155509095', 'svinya@gmail.com', 'svinya', 'Ж'),
       /* Контакты учеников */
       ('Скотомор', 'Светлана', 'Алексеевна', '+79256661010', 'skotomor@gmail.com', 'skotomor', 'Ж'),
       ('Хизмузмалат', 'Мухмалат', NULL, '+79107521320', 'muhmalat@gmail.com', 'muhma', 'М'),
       ('Головкин', 'Леонид', 'Юрьевич', '+79124405867', 'golovkin@gmail.com', 'golovkin', 'М'),
       ('Чернильница', 'Анастасия', 'Магамедовна', '+79152011488', 'nastyuhamaga@gmail.com', 'maga', 'Ж'),
       ('Панцырь', 'Станислав', 'Сергеевич', '+79104403211', 'panzer@gmail.com', 'panzer', 'М'),
       /* Контакты родителей */
       ('Глист', 'Вячеслав', 'Денисович', '+79109106672', 'glist@gmail.com', 'glist', 'М'),
       ('Смирнова', 'Мария', 'Васильевна', '+79101428856', 'smirnova@gmail.com', 'smirnova', 'Ж'),
       ('Человек', 'Петр', 'Сергеевич', '+79856403397', 'chelik@gmail.com', 'chelik', 'М'),
       ('Шнюк', 'Птицеслав', 'Сигизмундович', '+79659905544', 'shyukcipa@gmail.com', 'shnyuk', 'М'),
       ('Иванов', 'Иван', 'Иванович', '+79158008832', 'ivanov.i.i@gmail.com', 'ivanov', 'М');

-- добавляю сотрудников
INSERT INTO Employees(contacts_id) (SELECT ID FROM contacts WHERE id <= 5);

INSERT INTO Classes(class_number, leader_id, auditory_id)
VALUES ('11-А', 1, 1),
       ('10-Б', 2, 2);
    -- контакты учителей
-- добавляю родителей
INSERT INTO Parents(contacts_id) (SELECT ID FROM Contacts WHERE ID > 10 AND ID < 16);
-- добавляю учеников
INSERT INTO Students(class_id, contacts_id)
VALUES (5, 6), (5, 7),
       (5,8), (6,9), (6,10);
-- добавляю задания
INSERT INTO Tasks(SUBJECT_ID, CLASS_ID, TASK_TEXT, TEACHER_ID)
VALUES (1, 5, 'Страница 190 задачи 11-15', 1),
       (2, 5, 'Электричество и магнитизм', 1),
       (3, 5, 'Функции и методы C#, перегрузка, рекурсия', 1);
-- добавляю оценки
INSERT INTO Marks(student_id, mark, mark_date, teacher_id, task_id)
VALUES (6, 5, NOW(), 1, 4),
       (7, 5, NOW(), 1, 4),
       (8, 2, now(), 1, 4),

       (6, 2, NOW(), 1, 5),
       (7, 2, NOW(), 1, 5),
       (8, 2, now(), 1, 5),

       (6, 3, NOW(), 1, 6),
       (7, 5, NOW(), 1, 6),
       (8, 4, now(), 1, 6);
--добавляю расписание
INSERT INTO Timetable(day_of_week, lesson_time, class_id, subject_id, auditory_id, teacher_id)
VALUES (1, '10:00:00', 5, 1, 5, 1),
       (2, '10:00:00',5, 3, 6, 1);

-- связываю учеников и родителей
INSERT INTO Parent_student(parent_id, student_id)
VALUES (1,6), (2,7),
       (3,8), (4,9),
       (5,10);
-- связываю учителей и предметы
INSERT INTO Employee_Subject(employee_id, subject_id)
VALUES (1, 1), (1, 2), (1,3),
       (4, 1), (4, 2), (4,3),
       (2, 8), (2, 9), (2,10),
       (3, 11), (3, 12), (3,13), (3, 14),
       (5, 4), (5, 5), (5,18);

create table UserRoles(
    id SERIAL NOT NULL PRIMARY KEY,
    rolename VARCHAR(16) NOT NULL UNIQUE
);

CREATE TABLE Class_Subject(
  ID SERIAL NOT NULL PRIMARY KEY,
  Class_id INT NOT NULL,
  Subject_id INT NOT NULL,
  FOREIGN KEY (Class_id) REFERENCES Classes(ID),
  FOREIGN KEY (Subject_id) REFERENCES Subjects(ID)
);

INSERT INTO Class_Subject(Class_id, Subject_id)
VALUES (5, 3),
       (5, 2),
       (5,1);

INSERT INTO UserRoles(rolename)
VALUES ('employee'), ('student'), ('parent'), ('admin');

ALTER TABLE Contacts ADD COLUMN userrole_id INT;

UPDATE Contacts SET userrole_id = 1 WHERE ID IN (SELECT contacts_id FROM Employees);
UPDATE Contacts SET userrole_id = 2 WHERE ID IN (SELECT contacts_id FROM Students);
UPDATE Contacts SET userrole_id = 3 WHERE ID IN (SELECT contacts_id FROM Parents);

-- индексы
CREATE UNIQUE INDEX UX_Timetable_lesson_day_date ON Timetable(day_of_week, lesson_time);

-----------------------
-- изменения

ALTER TABLE Marks ADD COLUMN subject_id INT;

UPDATE Marks SET subject_id = 1 WHERE task_id = 4;
UPDATE Marks SET subject_id = 2 WHERE task_id = 5;
UPDATE Marks SET subject_id = 3 WHERE task_id = 6;

ALTER TABLE Marks
    ADD CONSTRAINT fk_marks_subjects FOREIGN KEY (subject_id) REFERENCES Subjects (id);

ALTER TABLE Marks
    ALTER COLUMN subject_id SET NOT NULL;

ALTER TABLE Tasks ADD COLUMN task_start_date timestamp;
ALTER TABLE Tasks ADD COLUMN task_deadline_date timestamp;

INSERT INTO Tasks(subject_id, class_id, task_text, teacher_id, task_start_date, task_deadline_date)
    VALUES(2, 5, 'Молекулярная физика. Как спариваются микробы?', 1, '2023-07-18', '2023-07-25')

INSERT INTO Marks(student_id, mark, mark_date, teacher_id, task_id, subject_id)
    VALUES(6, 4, '2023-07-18', 1, 5, 2);

-- TODO: снова добавить ограничение NOT NULL
ALTER TABLE Marks ALTER COLUMN task_id DROP NOT NULL;

-- YEAR = 2023; MONTH = 3
-- UPDATE Tasks SET task_start_date = '2023-03-10', task_deadline_date = '2023-03-18' WHERE id = 2;