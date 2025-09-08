-- ============================================
-- NODES
-- ============================================

INSERT INTO sync_nodes (name, url, type_id) VALUES ('Central',  'http://central:50010/api', 1);
INSERT INTO sync_nodes (name, url, type_id) VALUES ('Filial01', 'http://filial01:50020/api', 2);
INSERT INTO sync_nodes (name, url, type_id) VALUES ('Filial02', 'http://filial02:50030/api', 2);

-- ============================================
-- USER TYPES
-- ============================================

INSERT INTO sync_user_types (name, description) VALUES ('SystemAdmin', 'Administrador da Central');
INSERT INTO sync_user_types (name, description) VALUES ('SyncAPI', 'Comunicação Central <-> Remotes');
INSERT INTO sync_user_types (name, description) VALUES ('ExternalAPI', 'Integrações de terceiros');
INSERT INTO sync_user_types (name, description) VALUES ('HumanUser', 'Usuários humanos');

-- ============================================
-- USERS
-- ============================================

-- Usuários da Central para acessar cada remote
INSERT INTO users (type_id, username, password, description) VALUES (1,'admin', '1234', 'System Admin');
INSERT INTO users (type_id, username, password, description) VALUES (2, 'CAR01', '1234', 'Central Access Remote 01');
INSERT INTO users (type_id, username, password, description) VALUES (2, 'CAR02', '1234', 'Central Access Remote 02');

-- Usuários das remotes para a central
INSERT INTO users (type_id, username, password, description) VALUES (2, 'Remote01', '1234', 'Remote01 access Central');
INSERT INTO users (type_id, username, password, description) VALUES (2, 'Remote02', '1234', 'Remote02 access Central');

-- ============================================
-- ROUTES
-- ============================================

-- Central chama remotes
INSERT INTO sync_routes (source_node_id, target_node_id, user_id) VALUES (1, 2, 1); -- Central -> Filial01 usando CAR01
INSERT INTO sync_routes (source_node_id, target_node_id, user_id) VALUES (1, 3, 2); -- Central -> Filial02 usando CAR02

-- Remotes podem chamar Central
INSERT INTO sync_routes (source_node_id, target_node_id, user_id) VALUES (2, 1, 3); -- Filial01 -> Central usando Remote01
INSERT INTO sync_routes (source_node_id, target_node_id, user_id) VALUES (3, 1, 4); -- Filial02 -> Central usando Remote02

COMMIT;
