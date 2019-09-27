module.exports = {
  root: true,
  env: {
    node: true,
  },
  extends: [
    'plugin:vue/essential',
    '@vue/airbnb',
  ],
  rules: {
    'no-console': process.env.NODE_ENV === 'production' ? 'error' : 'off',
    'no-debugger': process.env.NODE_ENV === 'production' ? 'error' : 'off',
    'prefer-template': 'off',
    'operator-linebreak': 'off',
    'space-infix-ops': 'off',
    'no-await-in-loop': 'off',
    'arrow-parens': 'off',
    'arrow-body-style': 'off',
    'no-shadow': 'off',
    'no-param-reassign': 'off',
    'no-plusplus': 'off',
    'max-len': ['error', {
      code: 120,
      ignoreUrls: true
    }],
    'comma-dangle': 'off',
    'func-names': 'off',
    'object-curly-spacing': 'off',
    'object-curly-newline': 'off',
    'prefer-destructuring': 'off',
  },
  parserOptions: {
    parser: 'babel-eslint',
  },
};
