import psycopg2
from colorama import Fore, Back, Style, init
from faker import Faker
import secrets, string, random

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
                auditory_number VARCHAR(10) NOT NULL UNIQUE
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
                teacher_id INT NOT NULL,
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
                teacher_id INT NOT NULL,
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

            query += '''CREATE UNIQUE INDEX UX_Timetable_lesson_day_date ON Timetable(day_of_week, lesson_time);'''

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

        # employees
        with connection.cursor() as cursor:
            cursor.execute("select c.id from contacts c join userroles ur on c.userrole_id = ur.id where ur.rolename = 'employee'")
            insert_employees(connection, [row[0] for row in cursor.fetchall()])
        
        #parents
        with connection.cursor() as cursor:
            cursor.execute("select c.id from contacts c join userroles ur on c.userrole_id = ur.id where ur.rolename = 'parent'")
            insert_parents(connection, [row[0] for row in cursor.fetchall()])

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