-- 道德经句子数据插入脚本
-- 生成时间: 2026-02-26 13:48:45

-- 注意: 请确保数据库中存在生成UUID的函数，或替换为具体的ID值

INSERT INTO classic_sentence (id, chapter_id, content, audio_url, order_index, is_published) VALUES (CONCAT('SENT', LPAD(1, 8, '0')), 'CHAPTER001', '道可道，非常道。', '/audio/道德经/001.mp3', 1, 1);
INSERT INTO classic_sentence (id, chapter_id, content, audio_url, order_index, is_published) VALUES (CONCAT('SENT', LPAD(2, 8, '0')), 'CHAPTER001', '名可名，非常名。', '/audio/道德经/002.mp3', 2, 1);
INSERT INTO classic_sentence (id, chapter_id, content, audio_url, order_index, is_published) VALUES (CONCAT('SENT', LPAD(3, 8, '0')), 'CHAPTER001', '无名天地之始，有名万物之母。', '/audio/道德经/003.mp3', 3, 1);
INSERT INTO classic_sentence (id, chapter_id, content, audio_url, order_index, is_published) VALUES (CONCAT('SENT', LPAD(4, 8, '0')), 'CHAPTER001', '故常无欲，以观其妙；', '/audio/道德经/004.mp3', 4, 1);
INSERT INTO classic_sentence (id, chapter_id, content, audio_url, order_index, is_published) VALUES (CONCAT('SENT', LPAD(5, 8, '0')), 'CHAPTER001', '常有欲，以观其徼。', '/audio/道德经/005.mp3', 5, 1);
