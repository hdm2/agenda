package com.example.demo.repo;

import java.util.List;

import org.springframework.data.repository.CrudRepository;

import com.example.demo.model.Contato;

public interface ContatoRepository extends CrudRepository<Contato, Long> {
	List<Contato> findByTelefone(String telefone);
}