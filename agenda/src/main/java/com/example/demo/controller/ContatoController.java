package com.example.demo.controller;

import java.io.IOException;
import java.util.List;

import javax.servlet.http.HttpServletResponse;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.example.demo.message.Response;
import com.example.demo.model.Contato;
import com.example.demo.repo.ContatoRepository;

@RestController
public class ContatoController {

	@Autowired
	ContatoRepository repository;

	@RequestMapping(value = "postcontato", method = RequestMethod.POST)
	public Response postContato(@RequestBody Contato contato) {
		
		if (contato.getTelefone().length()==0) {
        	return new Response("Error"," Telefone não informado)");
		}
		if (contato.getTelefone().length()<8) {
        	return new Response("Error","Telefone "+contato.getTelefone()+" inválido(mínimo 8 caracteres)");
		}

		List<Contato> contatos = repository.findByTelefone(contato.getTelefone());
        if (contatos.size()>0) {
        	return new Response("Error","Telefone "+contato.getTelefone()+" já existe");
        }
		
		repository.save(new Contato(contato.getNome(), contato.getTelefone()));
		return new Response("Done",contato);
	}

	@RequestMapping("/findall")
	public Response findAll() {

		Iterable<Contato> contatos = repository.findAll();

		return new Response("Done", contatos);
	}

	@RequestMapping("/contato/{id}")
	public Response findContatoById(@PathVariable("id") long id) {

		Contato contato = repository.findOne(id);

		return new Response("Done", contato);
	}

	@RequestMapping("/postcontatofromtext/{text}")
	public Response postContatoFromText(@PathVariable("text") String text) {
		java.util.StringTokenizer fields = new java.util.StringTokenizer(text,"|");
		int index=0;
		String nome="";
		String telefone="";
		Contato contato;
		while (fields.hasMoreTokens()) {
			if (index==0) {
				nome=fields.nextToken();
				++index;
			} 
			else {
			    telefone=fields.nextToken();	
			}
		
		}
		contato = new Contato(nome,telefone);
		
		repository.save(contato);

		return new Response("Done", contato);
	}

	
	
	@RequestMapping("/deletecontato/{id}")
	public Response delete (@PathVariable("id") long id) {
		Contato contato = repository.findOne(id);
        repository.delete(contato);
		return new Response("Done", contato);
		
	}
	

	@RequestMapping("/findbytelefone/{telefone}")
	public Response findByTelefone(@PathVariable("telefone") String telefone) {

		List<Contato> contatos = repository.findByTelefone(telefone);
        if (contatos.size()==0) {
        	return new Response("Error",telefone);
        }
        else {
		    return new Response("Done", contatos.get(0));
        }
	}
	
}
