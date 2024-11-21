namespace FlatMoney.LocalDataBase
{
    public static class Tables
    {
        public const string FLATS_TABLE_NAME = "flats";
        public const string CLIENTS_TABLE_NAME = "clients";
        public const string RESERVATIONS_TABLE_NAME = "reservations";
        public const string SERVICES_TABLE_NAME = "services";
        public const string RESERVATION_SERVICES_TABLE_NAME = "reservation_services";
        public const string RESERVATION_CLIENTS_TABLE_NAME = "reservation_clients";
        public const string PAYMENTS_TABLE_NAME = "payments";
        public const string EXPENSE_TYPES_TABLE_NAME = "expense_types";
        public const string EXPENSES_TABLE_NAME = "expenses";

        public const string FlatType1 = "Арендная";
        public const string FlatType2 = "Своя";

        public const string ReservationShortType = "Краткосрочное";
        public const string ReservationLongType = "Долгосрочное";

        public const string PaymentType1 = "Наличными";
        public const string PaymentType2 = "Безнал";

        public const string PaymentName1 = "Предоплата";
        public const string PaymentName2 = "Аванс";
        public const string PaymentName3 = "Оплата";

        public const string CREATE_FLATS_STATEMENT = $"CREATE TABLE IF NOT EXISTS {FLATS_TABLE_NAME} ( " +
                                                     $"PK_flat_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                     $"flat_name VARCHAR(50) NOT NULL, " +
                                                     $"flat_type VARCHAR(8) NOT NULL DEFAULT '{FlatType1}', " +
                                                     $"rent_cost FLOAT, " +
                                                     $"rent_date TEXT, " +
                                                     $"rent_interval INTEGER, " +
                                                     $"rent_autopay BOOLEAN, " +
                                                     $"internet_cost FLOAT, " +
                                                     $"internet_date TEXT, " +
                                                     $"internet_interval INTEGER, " +
                                                     $"internet_autopay BOOLEAN, " +
                                                     $"address VARCHAR(250), " +
                                                     $"CHECK ( (flat_type = '{FlatType1}') OR (flat_type = '{FlatType2}') ) ); ";

        public const string CREATE_CLIENTS_STATEMENT = $"CREATE TABLE IF NOT EXISTS {CLIENTS_TABLE_NAME} (" +
                                                       $"PK_client_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                       $"client_name VARCHAR(50) NOT NULL, " +
                                                       $"client_lastname VARCHAR(50), " +
                                                       $"client_patronymic VARCHAR(50), " +
                                                       $"phone VARCHAR(25) UNIQUE, " +
                                                       $"email VARCHAR(100) UNIQUE, " +
                                                       $"passport VARCHAR(30) UNIQUE, " +
                                                       $"registration VARCHAR(250)); ";

        public const string CREATE_RESERVATIONS_STATEMENT = $"CREATE TABLE IF NOT EXISTS {RESERVATIONS_TABLE_NAME} (" +
                                                            $"PK_reservation_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                            $"FK_flat_id INT, " +
                                                            $"reservation_type VARCHAR(20) NOT NULL DEFAULT 'Краткосрочное', " +
                                                            $"checkin_date TEXT NOT NULL, " +
                                                            $"checkout_date TEXT NOT NULL, " +
                                                            $"guest_amount INT NOT NULL DEFAULT 1, " +
                                                            $"kid_amount INT NOT NULL DEFAULT 0, " +
                                                            $"dm_amount INT NOT NULL DEFAULT 1, " +
                                                            $"cost_per_amount FLOAT NOT NULL DEFAULT 0, " +
                                                            $"deposit_cost FLOAT NOT NULL DEFAULT 0, " +
                                                            $"deposit_status VARCHAR(30), " +
                                                            $"reservation_comment VARCHAR(250), " +
                                                            $"FOREIGN KEY (FK_flat_id) REFERENCES flats (PK_flat_id) ON DELETE SET NULL, " +
                                                            $"CHECK ( (reservation_type = '{ReservationShortType}') OR (reservation_type = '{ReservationLongType}') ) ); ";

        public const string CREATE_SERVICES_STATEMENT = $"CREATE TABLE IF NOT EXISTS {SERVICES_TABLE_NAME} (" +
                                                        $"PK_service_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                        $"service_name VARCHAR(50) NOT NULL DEFAULT 'Услуга', " +
                                                        $"service_cost FLOAT); ";

        public const string CREATE_RESERVATION_SERVICES_STATEMENT = $"CREATE TABLE IF NOT EXISTS {RESERVATION_SERVICES_TABLE_NAME} (" +
                                                                    $"PK_reservation_service_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                                    $"FK_reservation_id INT NOT NULL, " +
                                                                    $"FK_service_id INT, " +
                                                                    $"service_cost FLOAT NOT NULL DEFAULT 0, " +
                                                                    $"FOREIGN KEY (FK_reservation_id) REFERENCES reservations (PK_reservation_id) ON DELETE CASCADE, " +
                                                                    $"FOREIGN KEY (FK_service_id) REFERENCES services (PK_service_id) ON DELETE SET NULL); ";

        public const string CREATE_RESERVATION_CLIENTS_STATEMENT = $"CREATE TABLE IF NOT EXISTS {RESERVATION_CLIENTS_TABLE_NAME} (" +
                                                                   $"PK_reservation_client_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                                   $"FK_reservation_id INT NOT NULL, " +
                                                                   $"FK_client_id INT, " +
                                                                   $"client_name VARCHAR(50) NOT NULL DEFAULT 'Имя не установлено', " +
                                                                   $"FOREIGN KEY (FK_reservation_id) REFERENCES reservations (PK_reservation_id) ON DELETE CASCADE, " +
                                                                   $"FOREIGN KEY (FK_client_id) REFERENCES clients (PK_client_id) ON DELETE SET NULL); ";

        public const string CREATE_PAYMENTS_STATEMENT = $"CREATE TABLE IF NOT EXISTS {PAYMENTS_TABLE_NAME} (" +
                                                        $"PK_payment_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                        $"FK_reservation_id INT, " +
                                                        $"payment_name VARCHAR(50) NOT NULL DEFAULT 'Оплата за проживание', " +
                                                        $"payment_type VARCHAR(20) NOT NULL DEFAULT 'Безнал', " +
                                                        $"payment_date TEXT NOT NULL, " +
                                                        $"payment_cost FLOAT NOT NULL DEFAULT 0, " +
                                                        $"FOREIGN KEY (FK_reservation_id) REFERENCES reservations (PK_reservation_id) ON DELETE SET NULL, " +
                                                        $"CHECK ( (payment_name = '{PaymentName1}') OR (payment_name = '{PaymentName2}') OR (payment_name = '{PaymentName3}') )," +
                                                        $"CHECK ( (payment_type = '{PaymentType1}') OR (payment_type = '{PaymentType2}') ) ); ";

        public const string CREATE_EXPENSE_TYPES_STATEMENT = $"CREATE TABLE IF NOT EXISTS {EXPENSE_TYPES_TABLE_NAME} (" +
                                                             $"PK_expense_type_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                             $"expense_type_name VARCHAR(50) NOT NULL UNIQUE ); ";

        public const string CREATE_EXPENSES_STATEMENT = $"CREATE TABLE IF NOT EXISTS {EXPENSES_TABLE_NAME} (" +
                                                        $"PK_expense_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                        $"FK_expense_type_id INT, " +
                                                        $"expense_date TEXT NOT NULL, " +
                                                        $"expense_cost FLOAT NOT NULL DEFAULT 0, " +
                                                        $"FOREIGN KEY (FK_expense_type_id) REFERENCES expense_types (PK_expense_type_id) ON DELETE SET NULL); ";
    }
}
