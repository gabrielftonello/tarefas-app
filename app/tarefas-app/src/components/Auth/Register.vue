<template>
  <v-card class="mx-auto my-12" max-width="400">
    <v-card-title>Registrar</v-card-title>
    <v-card-text>
      <v-form @submit.prevent="aoEnviar" ref="formulario">
        <v-text-field
            label="Email"
            v-model="email"
            :rules="regrasEmail"
            required
        ></v-text-field>
        <v-text-field
            label="Senha"
            v-model="senha"
            :rules="regrasSenha"
            type="password"
            required
        ></v-text-field>
        <v-text-field
            label="Confirmar Senha"
            v-model="confirmarSenha"
            :rules="regrasConfirmarSenha"
            type="password"
            required
        ></v-text-field>
        <v-btn type="submit" color="primary" :loading="carregando">Registrar</v-btn>
      </v-form>
      <v-alert v-if="erro" type="error" dense>{{ erro }}</v-alert>
    </v-card-text>
  </v-card>
</template>

<script>
import { mapActions } from 'vuex';

export default {
  data() {
    return {
      email: '',
      senha: '',
      confirmarSenha: '',
      regrasEmail: [
        v => !!v || 'O campo Email deve ser preenchido',
        v => /.+@.+\..+/.test(v) || 'O Email deve ser válido',
      ],
      regrasSenha: [
        v => !!v || 'O campo Senha é obrigatório',
        v => v.length >= 6 || 'A senha deve ter pelo menos 6 caracteres',
        v => /[A-Z]/.test(v) || 'A senha deve conter pelo menos uma letra maiúscula',
        v => /\d/.test(v) || 'A senha deve conter pelo menos um número',
        v => /[^a-zA-Z0-9]/.test(v) || 'A senha deve conter pelo menos um caractere não alfanumérico',
      ],
      regrasConfirmarSenha: [
        v => !!v || 'Confirme sua senha',
        v => v === this.senha || 'As senhas devem coincidir',
      ],
      carregando: false,
      erro: '',
    };
  },
  watch: {
    senha(val) {
      this.$refs.formulario.validate();
    },
  },
  methods: {
    ...mapActions('auth', ['register']),
    async aoEnviar() {
      const valido = this.$refs.formulario.validate();
      if (valido) {
        this.carregando = true;
        this.erro = '';
        try {
          await this.register({ email: this.email, password: this.senha });
          this.$router.push({ name: 'Home' }).then(() => window.location.reload());
        } catch (err) {
          this.erro = err.response?.data?.message || 'Falha no registro';
        } finally {
          this.carregando = false;
        }
      }
    },
  },
};
</script>

<style scoped>
</style>
