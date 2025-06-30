use std::collections::HashMap;

pub struct FurlanPhoneticAlgorithm;

impl FurlanPhoneticAlgorithm {
    pub fn levenshtein(source: &str, target: &str) -> usize {
        let source_len = source.chars().count();
        let target_len = target.chars().count();
        if source_len == 0 {
            return target_len;
        }
        if target_len == 0 {
            return source_len;
        }
        let mut dist = vec![vec![0; target_len + 1]; source_len + 1];
        for i in 0..=source_len {
            dist[i][0] = i;
        }
        for j in 0..=target_len {
            dist[0][j] = j;
        }
        let source_chars: Vec<char> = source.chars().collect();
        let target_chars: Vec<char> = target.chars().collect();
        for i in 1..=source_len {
            for j in 1..=target_len {
                let cost = if source_chars[i - 1] == target_chars[j - 1] {
                    0
                } else {
                    // TODO: implement Friulian vowel logic
                    1
                };
                dist[i][j] = std::cmp::min(
                    std::cmp::min(dist[i - 1][j] + 1, dist[i][j - 1] + 1),
                    dist[i - 1][j - 1] + cost,
                );
            }
        }
        dist[source_len][target_len]
    }

    pub fn sort_friulian(words: &mut Vec<String>) {
        words.sort_by_key(|w| Self::translate_word_for_sorting(w));
    }

    pub fn get_phonetic_hashes_by_word(word: &str) -> (String, String) {
        Self::get_phonetic_hashes_by_original(&Self::prepare_original_word(word))
    }

    fn prepare_original_word(original: &str) -> String {
        let mut s = original.replace(|c: char| c.is_whitespace(), "");
        s = s.to_lowercase();
        // TODO: implement all regex replacements as in C#
        s
    }

    fn get_phonetic_hashes_by_original(original: &str) -> (String, String) {
        let mut first_hash = original.to_string();
        let mut second_hash = original.to_string();
        // TODO: implement all regex replacements as in C#
        (first_hash, second_hash)
    }

    fn translate_word_for_sorting(word: &str) -> String {
        let original_chars = "0123456789âäàáÄÁÂÀAaBCçÇDéêëèÉÊËÈEeFGHïîìíÍÎÏÌIiJKLMNôöòóÓÔÒÖOoPQRSTÚÙÛÜúûùüuUVWXYZ";
        let sorted_chars = "0123456789aaaaaaaaaabcccdeeeeeeeeeefghiiiiiiiiiijklmnoooooooooopqrstuuuuuuuuuuvwxyz";
        let mut map = HashMap::new();
        for (o, s) in original_chars.chars().zip(sorted_chars.chars()) {
            map.insert(o, s);
        }
        word.chars()
            .map(|c| *map.get(&c).unwrap_or(&c))
            .collect::<String>()
            .replace("^'s", "s")
    }
}
