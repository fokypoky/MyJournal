import psycopg2
from colorama import Fore, Back, Style, init
from faker import Faker
import secrets, string, random, math
from datetime import datetime, timedelta

init()

faker = Faker('ru_RU')

class ConnectionSettings:
    def __init__(self, host, port, database, user, password) -> None:
        self.host = host
        self.port = port
        self.database = database
        self.user = user
        self.password = password
    
    def is_valid(self) -> bool:
        return self.host is not None and self.port is not None and self.database is not None and self.user is not None and self.password is not None

    def print(self) -> None:
        print(self.host, self.port, self.database, self.user, self.password)

    def copy(self):
        return ConnectionSettings(host=self.host, port=self.port, database=self.database, user=self.user, password=self.password)

class Contacts:
    def __init__(self, surname, name, midname, phone_number, email, password, sex, userrole_id) -> None:
        self.surname = surname
        self.name = name
        self.midname = midname
        self.phone_number = phone_number
        self.email = email
        self.password = password
        self.sex = sex
        self.userrole_id = userrole_id

class Class:
    def __init__(self, class_number, leader_id, auditory_id) -> None:
        self.class_number = class_number
        self.leader_id = leader_id
        self.auditory_id = auditory_id

class Student:
    def __init__(self, class_id, contacts_id, id=None) -> None:
        self.class_id = class_id
        self.contacts_id = contacts_id
        self.id = id

class ParentStudent:
    def __init__(self, student_id, parent_id) -> None:
        self.student_id = student_id
        self.parent_id = parent_id

class Timetable:
    def __init__(self, day_of_week, lesson_time, class_id, subject_id, auditory_id, teacher_id) -> None:
        self.day_of_week = day_of_week
        self.lesson_time = lesson_time
        self.class_id = class_id
        self.subject_id = subject_id
        self.auditory_id = auditory_id
        self.teacher_id = teacher_id

class ClassSubject:
    def __init__(self, class_id, subject_id) -> None:
        self.class_id = class_id
        self.subject_id = subject_id

class EmployeeSubject:
    def __init__(self, employee_id, subject_id) -> None:
        self.employee_id = employee_id
        self.subject_id = subject_id

class Task:
    def __init__(self, subject_id, class_id, task_text, teacher_id, task_start_date, task_deadline_date, id=None) -> None:
        self.subject_id = subject_id
        self.class_id = class_id
        self.task_text = task_text
        self.teacher_id = teacher_id
        self.task_start_date = task_start_date
        self.task_deadline_date = task_deadline_date
        self.id = id

class Mark:
    def __init__(self, student_id, mark, mark_date, teacher_id, subject_id) -> None:
        self.student_id = student_id
        self.mark = mark
        self.mark_date = mark_date
        self.teacher_id = teacher_id
        self.subject_id = subject_id

def print_sepparator_line_1() -> None:
    print('--------------------')

def print_sepparator_line_2() -> None:
    print('----------')

def configure_connection(con_settings) -> ConnectionSettings:
    settings_copy = con_settings.copy()

    while True:
        print_sepparator_line_1()
        print(f'Host: {settings_copy.host}\nPort: {settings_copy.port}\nDatabase: {settings_copy.database}')
        print(f'User: {settings_copy.user}\nPassword: {settings_copy.password}')
        print_sepparator_line_2()
        
        ch = int(input('1. Edit\n2. Back\n'))

        if ch == 1:
            settings_copy.host = input('New host: ')
            settings_copy.port = int(input('New port: '))
            settings_copy.database = input('New database: ')
            settings_copy.user = input('New user: ')
            settings_copy.password = input('New password: ')

            settings_copy.print()

            try:
                connection = psycopg2.connect(
                    host=settings_copy.host,
                    port=settings_copy.port,
                    database=settings_copy.database,
                    user=settings_copy.user,
                    password=settings_copy.password
                )
            except:
                print(Fore.RED + "Can't connect with new options" + Style.RESET_ALL)
            finally:
                connection.close()
        if ch == 2:
            return settings_copy

def create_database_structure(con_settings) -> None:
    try:
        connection = psycopg2.connect(
            host=con_settings.host,
            port=con_settings.port,
            database=con_settings.database,
            user=con_settings.user,
            password=con_settings.password
        )
        connection.autocommit = True

        with connection.cursor() as cursor:
            query = '''CREATE TABLE userroles(
                id SERIAL NOT NULL PRIMARY KEY,
                rolename VARCHAR(16) NOT NULL UNIQUE
            );'''

            query += '''CREATE TABLE contacts(
                id SERIAL NOT NULL PRIMARY KEY,
                surname VARCHAR(50) NOT NULL,
                name VARCHAR(50) NOT NULL,
                midname VARCHAR(50),
                phone_number VARCHAR(15) NOT NULL UNIQUE,
                email VARCHAR(100) NOT NULL UNIQUE,
                password VARCHAR(20) NOT NULL,
                sex VARCHAR(1) NOT NULL,
                userrole_id INT NOT NULL,
                FOREIGN KEY (userrole_id) REFERENCES userroles(id)
            );'''

            query += '''CREATE TABLE employees(
                id SERIAL NOT NULL PRIMARY KEY,
                contacts_id INT NOT NULL,
                FOREIGN KEY (contacts_id) REFERENCES contacts(id)
            );'''

            query += '''CREATE TABLE auditories(
                id SERIAL NOT NULL PRIMARY KEY,
                auditory_number VARCHAR(10) UNIQUE
            );'''

            query += '''CREATE TABLE classes(
                id SERIAL NOT NULL PRIMARY KEY,
                class_number VARCHAR(5) NOT NULL UNIQUE,
                leader_id INT,
                auditory_id INT,
                FOREIGN KEY (leader_id) REFERENCES employees(id),
                FOREIGN KEY (auditory_id) REFERENCES auditories(id)
            );'''

            query += '''CREATE TABLE students(
                id SERIAL NOT NULL PRIMARY KEY,
                class_id INT NOT NULL,
                contacts_id INT NOT NULL,
                FOREIGN KEY (class_id) REFERENCES classes(id),
                FOREIGN KEY (contacts_id) REFERENCES contacts(id)
            );'''

            query += '''CREATE TABLE parents(
                id SERIAL NOT NULL PRIMARY KEY,
                contacts_id INT NOT NULL,
                FOREIGN KEY (contacts_id) REFERENCES contacts(id)
            );'''

            query += '''CREATE TABLE parent_student(
                id SERIAL NOT NULL PRIMARY KEY,
                parent_id INT NOT NULL,
                student_id INT NOT NULL,
                FOREIGN KEY (parent_id) REFERENCES parents(id),
                FOREIGN KEY (student_id) REFERENCES students(id)
            );'''

            query += '''CREATE TABLE subjects(
                id SERIAL NOT NULL PRIMARY KEY,
                subject_title VARCHAR(50) NOT NULL UNIQUE
            );'''

            query += '''CREATE TABLE employee_subject(
                id SERIAL NOT NULL PRIMARY KEY,
                employee_id INT NOT NULL,
                subject_id INT NOT NULL,
                FOREIGN KEY (employee_id) REFERENCES employees(id),
                FOREIGN KEY (subject_id) REFERENCES subjects(id)
            );'''

            query += '''CREATE TABLE tasks(
                id SERIAL NOT NULL PRIMARY KEY,
                subject_id INT NOT NULL,
                class_id INT NOT NULL,
                task_text VARCHAR(200) NOT NULL,
                teacher_id INT,
                task_start_date TIMESTAMP,
                task_deadline_date TIMESTAMP,
                FOREIGN KEY (subject_id) REFERENCES subjects(id),
                FOREIGN KEY (class_id) REFERENCES classes(id),
                FOREIGN KEY (teacher_id) REFERENCES employees(id)
            );'''
            
            query += '''CREATE TABLE marks(
                id SERIAL NOT NULL PRIMARY KEY,
                student_id INT NOT NULL,
                mark INT NOT NULL,
                mark_date TIMESTAMP NOT NULL,
                teacher_id INT,
                task_id INT,
                subject_id INT NOT NULL,
                FOREIGN KEY (student_id) REFERENCES students(id),
                FOREIGN KEY (teacher_id) REFERENCES employees(id),
                FOREIGN KEY (task_id) REFERENCES tasks(id),
                FOREIGN KEY (subject_id) REFERENCES subjects(id)
            );'''

            query += '''CREATE TABLE timetable(
                id SERIAL NOT NULL PRIMARY KEY,
                day_of_week INT NOT NULL,
                lesson_time TIME NOT NULL,
                class_id INT NOT NULL,
                subject_id INT NOT NULL,
                auditory_id INT NOT NULL,
                teacher_id INT,
                FOREIGN KEY (class_id) REFERENCES classes(id),
                FOREIGN KEY (subject_id) REFERENCES subjects(id),
                FOREIGN KEY (auditory_id) REFERENCES auditories(id),
                FOREIGN KEY (teacher_id) REFERENCES employees(id)
            );'''

            query += '''CREATE TABLE class_subject(
                id SERIAL NOT NULL PRIMARY KEY,
                class_id INT NOT NULL,
                subject_id INT NOT NULL,
                FOREIGN KEY (class_id) REFERENCES classes(id),
                FOREIGN KEY (subject_id) REFERENCES subjects(id)
            );'''

            cursor.execute(query)
            print(Fore.LIGHTGREEN_EX + 'Done' + Style.RESET_ALL)

    except Exception as e:
        print(Fore.RED + 'An error occurred' + Style.RESET_ALL)
        
        ch = int(input('Show details?\n1. Yes\n2. No\n'))

        if ch == 1:
            print(str(e))
    finally:
        connection.close()

def drop_database_structure(con_settings) -> None:
    try:
        connection = psycopg2.connect(
            host=con_settings.host,
            port=con_settings.port,
            database=con_settings.database,
            user=con_settings.user,
            password=con_settings.password
        )
        connection.autocommit = True
        
        with connection.cursor() as cursor:
            query = '''DROP TABLE auditories, class_subject, classes, contacts,
                employee_subject, employees, marks, parent_student,
                parents, students, subjects, tasks, timetable, userroles
                CASCADE;'''

            cursor.execute(query)
            print(Fore.LIGHTGREEN_EX + 'Done' + Style.RESET_ALL)

    except Exception as e:
        print(Fore.RED + 'An error occurred' + Style.RESET_ALL)
        
        ch = int(input('Show details?\n1. Yes\n2. No\n'))

        if ch == 1:
            print(str(e))

    finally:
        connection.close()

def generate_password() -> str:
    alphabet = string.ascii_letters + string.digits
    return ''.join(secrets.choice(alphabet) for i in range (20))

def handle_phone_number(phone_number) -> str:
    return phone_number.replace('(', '').replace(')', '').replace(' ', '').replace('+', '').replace('-', '')

# FIXME: иногда имена распознаются как фамилии
def generate_contacts(roles, contacts_count) -> list:
    contacts = []
    for i in range(contacts_count):
        sex = 'М' if random.randint(0, 1) == 0 else 'Ж'
        splitted_full_name = (faker.name_male() if sex == 'М' else faker.name_female()).split(' ')
        
        surname = splitted_full_name[0]
        name = splitted_full_name[1]
        midname = splitted_full_name[2]
        phone_number = handle_phone_number(faker.unique.phone_number())
        password = generate_password()
        email = faker.unique.email()
        userrole_id = int(random.choice(roles)[0])
        contacts.append(Contacts(surname=surname, name=name, midname=midname,phone_number=phone_number, email=email, password=password, sex=sex, userrole_id=userrole_id))
    
    return contacts

def get_employees_id(connection) -> list:
    with connection.cursor() as cursor:
        cursor.execute('select id from employees')
        return [row[0] for row in cursor.fetchall()]

def get_auditories_id(connection) -> list:
    with connection.cursor() as cursor:
        cursor.execute('select id from auditories')
        return [row[0] for row in cursor.fetchall()]

def get_subjects_id(connection) -> list:
    with connection.cursor() as cursor:
        cursor.execute('select id from subjects')
        return [row[0] for row in cursor.fetchall()]

def get_ids_from_table(connection, table_name: str) -> list:
    with connection.cursor() as cursor:
        cursor.execute(f'select id from {table_name}')
        return [row[0] for row in cursor.fetchall()]

def generate_auditories(count) -> list:
    auditories = []
    for number in range(1, count + 1):
        for letter in 'АБВ':
            auditories.append(str(number) + letter)
    
    return auditories

def get_class_subjects(connection) -> list:
    class_subjects = []
    with connection.cursor() as cursor:
        cursor.execute('select class_id, subject_id from class_subject')
        for data in cursor.fetchall():
            class_subjects.append(ClassSubject(data[0], data[1]))
    return class_subjects

def get_employee_subjects(connection) -> None:
    employee_subjects = []
    with connection.cursor() as cursor:
        cursor.execute('select employee_id, subject_id from employee_subject')
        for data in cursor.fetchall():
            employee_subjects.append(EmployeeSubject(data[0], data[1]))
    return employee_subjects

def get_tasks(connection) -> list:
    tasks = []
    with connection.cursor() as cursor:
        cursor.execute('select * from tasks')
        for data in cursor.fetchall():
            tasks.append(Task(id=data[0], subject_id=data[1],
                              class_id=data[2], task_text=data[3],
                              teacher_id=data[4], task_start_date=data[5],
                              task_deadline_date=data[6]))
    return tasks

def get_students(connection) -> list:
    students = []
    with connection.cursor() as cursor:
        cursor.execute('select * from students')
        for data in cursor.fetchall():
            students.append(Student(id=data[0], class_id=data[1], contacts_id=data[2]))
    
    return students

def generate_classes(auditories_id: list, employees_id: list) -> list:
    classes = []
    auditories_id_copy = auditories_id.copy()
    employees_id_copy = employees_id.copy()

    random.shuffle(auditories_id_copy)
    random.shuffle(employees_id_copy)

    for number in range(5, 12):
        for letter in 'АБВ':
            classes.append(Class(class_number= str(number) + letter, 
                                leader_id= employees_id_copy.pop(),
                                auditory_id= auditories_id_copy.pop()))
    
    return classes

def generate_students(contacts_id:list, classes_id: list) -> list:
    students = []
    
    num_classes = len(classes_id)
    num_students = len(contacts_id)

    avg_students_per_class = num_students // num_classes

    class_student_mapping = {}
    for i, class_id in enumerate(classes_id):
        start_index = i * avg_students_per_class
        end_index = (i + 1) * avg_students_per_class if i < len(classes_id) - 1 else len(contacts_id)
        students_in_class = contacts_id[start_index:end_index]
        class_student_mapping[class_id] = students_in_class
    
    for class_id in class_student_mapping.keys():
        for contact_id in class_student_mapping[class_id]:
            students.append(Student(class_id=class_id, contacts_id=contact_id))
    
    return students

def generate_timetable(class_subjects, class_ids, auditory_ids, employee_subjects) -> list:
    timetables = []

    lessons_times = {
        1: '09:00:00', 2: '09:55:00', 3: '10:50:00', 
        4: '11:45:00', 5: '12:40:00', 6: '13:35:00',
        7: '14:30:00', 8: '15:25:00'
    }

    for class_id in class_ids:
        current_class_subject = list(filter(lambda f: f.class_id == class_id, class_subjects))
        for day in range(1, 7): # day of week number
            lessons_count = random.randint(0, 8)
            current_day_subjects = []
            for i in range(lessons_count):
                current_day_subjects.append(random.choice(current_class_subject))
            
            for i in range(0, len(current_day_subjects)):
                # обеспечиваю, чтоб один учитель не вел 2 предмета одновременно в одной аудитории
                while True:
                    teacher_id = random.choice(list(filter(lambda f: f.subject_id == current_day_subjects[i].subject_id, employee_subjects))).employee_id
                    lesson_time = lessons_times[i + 1]
                    
                    auditory_id = random.choice(auditory_ids)

                    if len(list(filter(lambda t: t.lesson_time == lesson_time 
                        and t.day_of_week == day and (
                        t.auditory_id == auditory_id or t.teacher_id == teacher_id
                    ), timetables))) == 0:
                        timetables.append(Timetable(day, lesson_time, class_id, current_class_subject[i].subject_id, auditory_id, teacher_id))
                        break

    return timetables

def truncate_task_text(text) -> str:
    return text[:197] + '...' if len(text) > 200 else text

def generate_tasks(classes_ids, employee_subject, timetables) -> list:
    tasks = []
    for month in range(3, 10):
        for class_id in classes_ids:
            class_timetable = list(filter(lambda t: t.class_id == class_id, timetables))
            date = datetime.strptime(f'2023-{month}-1', '%Y-%m-%d')

            for day in range(1, 7):
                current_day_subject_ids = list(filter(lambda t: t.day_of_week == day, class_timetable))

                for current_day_subject_id in current_day_subject_ids:
                    task_start_date = date
                    task_deadline_date = date + timedelta(days=7)
                    teacher_id = random.choice(list(filter(lambda es: es.subject_id == current_day_subject_id.subject_id, employee_subject))).employee_id
                    tasks.append(Task(current_day_subject_id.subject_id, class_id, truncate_task_text(faker.text()), teacher_id, task_start_date, task_deadline_date))
    return tasks

def generate_marks(tasks, students) -> list:
    marks = []
    for task in tasks:
        current_task_students = list(filter(lambda s: s.class_id == task.class_id, students))
        for student in current_task_students:
            mark_value = random.randint(2, 5)
            marks.append(Mark(student_id=student.id, mark=mark_value, mark_date=task.task_deadline_date,
                              teacher_id=task.teacher_id, subject_id=task.subject_id))
    return marks


def insert_contacts(connection, contacts) -> None:
    with connection.cursor() as cursor:
        query = 'insert into contacts(surname, name, midname, phone_number, email, password, sex, userrole_id) values'
        for contact in contacts:
            query += f"""('{contact.surname}', '{contact.name}', '{contact.midname}', '{contact.phone_number}',"""
            query += f"""'{contact.email}', '{contact.password}', '{contact.sex}', {contact.userrole_id}),"""
        cursor.execute(query[:-1])

def insert_employees(connection, employees_ids) -> None:
    with connection.cursor() as cursor:
        query = 'insert into employees(contacts_id) values'
        for id in employees_ids:
            query += f'({id}),'
        cursor.execute(query[:-1])

def insert_parents(connection, parents_ids) -> None:
    with connection.cursor() as cursor:
        query = 'insert into parents(contacts_id) values'
        for id in parents_ids:
            query += f'({id}),'
        cursor.execute(query[:-1])

def insert_auditories(connection, auditories) -> None:
    with connection.cursor() as cursor:
        query = '''insert into auditories(auditory_number) values'''
        for auditory in auditories:
            query += f'''('{auditory}'),'''
        cursor.execute(query[:-1])

def insert_classes(connection, classes) -> None:
    with connection.cursor() as cursor:
        query = 'insert into classes(class_number, leader_id, auditory_id) values'
        for _class in classes:
            query += f'''('{_class.class_number}', {_class.leader_id}, {_class.auditory_id}),'''
        cursor.execute(query[:-1])

def insert_subjects(connection) -> None:
    subjects = [
        'Математика', 'Алгебра', "Геометрия", "Информатика",
        "Природоведение", "Окружающий мир", "География", "Биология",
        "Физика", "Химия", "ОБЖ", "История", "Обществознание", 
        "Родной язык", "Родная литература", "Русский язык", "Литературное чтение",
        "Литература", "Иностранный язык", "Технология", "Черчение", "Индивидуальный проект",
        "Физическая культура", "Музыка", "Изобразительное искусство", "Истоки"
    ]
    with connection.cursor() as cursor:
        query = 'insert into subjects(subject_title) values'
        for subject in subjects:
            query += f'''('{subject}'),'''
        cursor.execute(query[:-1])

def insert_students(connection, students: list) -> None:
    with connection.cursor() as cursor:
        query = 'insert into students(class_id, contacts_id) values'
        for student in students:
            query += f'''({student.class_id}, {student.contacts_id}),'''
        cursor.execute(query[:-1])

def insert_timetable(connection, timetables) -> None:
    with connection.cursor() as cursor:
        query = 'insert into timetable(day_of_week, lesson_time, class_id, subject_id, auditory_id, teacher_id) values'
        for timetable in timetables:
            query += f'''({timetable.day_of_week}, '{timetable.lesson_time}', 
            {timetable.class_id}, {timetable.subject_id}, {timetable.auditory_id}, {timetable.teacher_id}),'''
        cursor.execute(query[:-1])

def insert_tasks(connection, tasks) -> None:
    with connection.cursor() as cursor:
        query = 'insert into tasks(subject_id, class_id, task_text, teacher_id, task_start_date, task_deadline_date) values'
        for task in tasks:
            query += f'''({task.subject_id}, {task.class_id}, '{task.task_text}', {task.teacher_id}, '{task.task_start_date}',
            '{task.task_deadline_date}'),'''
        cursor.execute(query[:-1])

def insert_marks(connection, marks) -> None:
    with connection.cursor() as cursor:
        query = 'insert into marks(student_id, mark, mark_date, teacher_id, subject_id) values'
        for mark in marks:
            query += f'''({mark.student_id}, {mark.mark}, '{mark.mark_date}', {mark.teacher_id}, {mark.subject_id}),'''
        cursor.execute(query[:-1])

def match_employees_subjects(connection, employees_id: list, subjects_id: list) -> None:
    with connection.cursor() as cursor:
        query = 'insert into employee_subject(employee_id, subject_id) values'
        for employee_id in employees_id:
            subjects_id_copy = subjects_id.copy()
            random.shuffle(subjects_id_copy)
            
            for i in range(3):
                subject_id = subjects_id_copy.pop()
                query += f'({employee_id}, {subject_id}),'
        cursor.execute(query[:-1])

def match_classes_subjects(connection, classes_id: list, subjects_id: list) -> None:
    with connection.cursor() as cursor:
        query = 'insert into class_subject(class_id, subject_id) values'
        for class_id in classes_id:
            subjects_id_copy = subjects_id.copy()
            random.shuffle(subjects_id_copy)

            for i in range(10):
                query += f'''({class_id}, {subjects_id_copy.pop()}),'''
        
        cursor.execute(query[:-1])

def match_students_with_parents(connection, student_ids: list, parent_ids: list) -> None:
    parent_students = []

    num_students = len(student_ids)
    num_parents = len(parent_ids)

    students_per_parent_count = math.ceil(num_students / num_parents)

    for parent_id in parent_ids:
        for i in range(students_per_parent_count):
            if len(student_ids) == 0:
                break
            parent_students.append(ParentStudent(student_ids.pop(), parent_id))

        if len(student_ids) == 0:
                break
    
    with connection.cursor() as cursor:
        query = 'insert into parent_student(parent_id, student_id) values'
        for parent_student in parent_students:
            query += f'''({parent_student.parent_id}, {parent_student.student_id}),'''
        cursor.execute(query[:-1])

def fill_test_data(con_settings) -> None:
    try:
        connection = psycopg2.connect(
                host=con_settings.host,
                port=con_settings.port,
                database=con_settings.database,
                user=con_settings.user,
                password=con_settings.password                      
        )
        connection.autocommit = True

        # roles
        roles = []
        with connection.cursor() as cursor:
            cursor.execute("insert into userroles(rolename) values('employee'), ('student'), ('parent'), ('admin')")
            cursor.execute("select * from userroles")
            for data in cursor.fetchall():
                roles.append([data[0], data[1]])

        # contacts
        default_contacts = generate_contacts(list(filter(lambda role: role[1] != 'admin', roles)), 500)
        admin_contacts = generate_contacts(list(filter(lambda role: role[1] == 'admin', roles)), 15)

        insert_contacts(connection, default_contacts + admin_contacts)

        # subjects
        insert_subjects(connection)

        # employees
        with connection.cursor() as cursor:
            cursor.execute("select c.id from contacts c join userroles ur on c.userrole_id = ur.id where ur.rolename = 'employee'")
            insert_employees(connection, [row[0] for row in cursor.fetchall()])
        
        match_employees_subjects(connection, get_employees_id(connection), get_subjects_id(connection))

        #parents
        with connection.cursor() as cursor:
            cursor.execute("select c.id from contacts c join userroles ur on c.userrole_id = ur.id where ur.rolename = 'parent'")
            insert_parents(connection, [row[0] for row in cursor.fetchall()])

        # auditories
        auditories = generate_auditories(15)
        insert_auditories(connection, auditories)

        # classes
        classes = generate_classes(get_auditories_id(connection), get_employees_id(connection))
        insert_classes(connection, classes)
        match_classes_subjects(connection, get_ids_from_table(connection, 'classes'), get_ids_from_table(connection, 'subjects'))

        # students
        with connection.cursor() as cursor:
            cursor.execute("select c.id from contacts c join userroles ur on c.userrole_id = ur.id where ur.rolename = 'student'")
            student_contacts_id = [row[0] for row in cursor.fetchall()]
            students = generate_students(student_contacts_id, get_ids_from_table(connection, 'classes'))
            insert_students(connection, students)

        match_students_with_parents(connection, get_ids_from_table(connection, 'students'), get_ids_from_table(connection, 'parents'))

        # timetable
        timetable = generate_timetable(get_class_subjects(connection), get_ids_from_table(connection, 'classes'), 
                                       get_ids_from_table(connection, 'auditories'), get_employee_subjects(connection))
        
        insert_timetable(connection, timetable)

        # tasks
        tasks = generate_tasks(get_ids_from_table(connection, 'classes'), get_employee_subjects(connection), timetable)
        insert_tasks(connection, tasks)

        marks = generate_marks(get_tasks(connection), get_students(connection))
        insert_marks(connection, marks)

    except Exception as e:
        print(Fore.RED + 'An error occurred' + Style.RESET_ALL)
        
        ch = int(input('Show details?\n1. Yes\n2. No\n'))

        if ch == 1:
            print(str(e))

    finally:
        connection.close()

def main() -> None:
    print(Fore.YELLOW + 'Before using the script, make sure that the database you are going to use has already been created and is empty' + Style.RESET_ALL)
    
    con_settings = ConnectionSettings(host='localhost', port='5432', database='MyJournalDB', user='postgres', password='toor')
    # TODO: for elephant sql test
    #region elephant test
    con_settings.host = 'mouse.db.elephantsql.com'
    con_settings.database = 'spmluhhu'
    con_settings.user = 'spmluhhu'
    con_settings.password = 'Rr10RlTEkrsRqcjIz4SlTk-w7dU1hZjF'
    #endregion


    while True:
        print_sepparator_line_1()
        print('MENU')
        print_sepparator_line_1()

        ch = int(input('1. Create database structure\n2. Delete database\n3. Fill test data\n4. Connection settings\n5. Exit\n'))

        if ch == 1:
            create_database_structure(con_settings)
        
        if ch == 2:
            drop_database_structure(con_settings)
        
        if ch == 3:
            fill_test_data(con_settings)
        
        if ch == 4:
            con_settings = configure_connection(con_settings=con_settings)

        if ch == 5:
            break

main()